using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HLab.Base.ReactiveUI;
using HLab.Erp.Acl;
using HLab.Erp.Base;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Erp.Lims.Analysis.FormClasses;
using HLab.Erp.Lims.Analysis.Products;
using HLab.Erp.Lims.Analysis.Samples;
using HLab.Erp.Lims.Analysis.Samples.SampleMovements;
using HLab.Erp.Lims.Analysis.Samples.SampleTests;
using HLab.Erp.Workflows.Models;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Avalonia.Samples;

public class SampleViewModelDesign : SampleViewModel.Design
{
}

public class SampleViewModel : ListableEntityViewModel<Sample>
{
    SampleViewModel() : base(null)
    {
    }
    public class Design : SampleViewModel, IDesignViewModel
    {
        public new Sample Model { get; } = Sample.DesignModel;
    }

    public Type ListProductType => typeof(ProductsListPopupViewModel);

    readonly Func<Sample, IDataLocker<Sample>, SampleWorkflow> _getSampleWorkflow;
    public IDocumentPresenter DocumentPresenter { get; }
    readonly IDocumentService _docs;
    readonly Func<Sample, SampleSampleTestListViewModel> _getTests;
    readonly Func<Sample, SampleMovementsListViewModel> _getMovements;
    readonly Func<Sample, SampleFormsListViewModel> _getForms;
    readonly Func<int, SampleAuditTrailViewModel> _getAudit;


    public SampleViewModel(
        Injector i,
        Func<Sample, SampleSampleTestListViewModel> getTests,
        Func<Sample, SampleMovementsListViewModel> getMovements,
        Func<Sample, SampleFormsListViewModel> getForms,
        Func<int, SampleAuditTrailViewModel> getAudit,
        Func<Sample, IDataLocker<Sample>, SampleWorkflow> getSampleWorkflow,
        IDocumentPresenter documentPresenter,
        IDocumentService docs
        ) : base(i)
    {
        _docs = docs;
        _getAudit = getAudit;
        _getSampleWorkflow = getSampleWorkflow;
        DocumentPresenter = documentPresenter;
        _getTests = getTests;
        _getMovements = getMovements;
        _getForms = getForms;

        _isReadOnly = this.WhenAnyValue(e => e.EditMode, editMode => !editMode)
            .ToProperty(this, e => e.IsReadOnly);

        this.WhenAnyValue(e => e.AuditDetail, e => e.AuditTrail)
            .Subscribe(e => OnAuditDetail(e.Item1, e.Item2));

        _auditTrail = this
            .WhenAnyValue(e => e.Model)
            .WhereNotNull()
            .Select(e => _getAudit?.Invoke(e.Id))
            .ToProperty(this, e => e.AuditTrail);


        this.WhenAnyValue(e => e.Locker.IsActive)
            .Subscribe(isActive =>
            {
                if (Tests != null)
                {
                    Tests.EditMode = isActive;
                }
            });

        _subTitle = this.WhenAnyValue(
            e => e.Model.Customer.Name,
            e => e.Model.Product.Caption,
                (customer, product) => $"{(customer ?? "{Customer}")}\n{(product ?? "{Product}")}")
            .ToProperty(this, e => e.SubTitle);

        _customerVisibility = this.WhenAnyValue(e => e.Injected.Acl.Connection.User)
            .Select(u => Injected.Acl.IsGranted(ErpRights.ErpViewCustomer))
            .ToProperty(this, e => e.CustomerVisibility);

        _editMode = this.WhenAnyValue(
                e => e.Locker.IsActive,
                e => e.Workflow.CurrentStage,
                (isActive, stage) => IsStageActive(isActive, stage,
                    SampleWorkflow.Reception,
                    AnalysisRights.AnalysisReceptionSign)
            )
            .ToProperty(this, e => e.EditMode);

        _monographMode = this.WhenAnyValue(
            e => e.Locker.IsActive,
            e => e.Workflow.CurrentStage,
            (isActive, stage) => IsStageActive(isActive, stage,
                SampleWorkflow.Monograph,
                AnalysisRights.AnalysisMonographSign)
            )
            .ToProperty(this, e => e.MonographMode);

        _productionMode = this.WhenAnyValue(
            e => e.Locker.IsActive,
            e => e.Workflow.CurrentStage,
            (isActive, stage) => IsStageActive(isActive, stage,
            SampleWorkflow.Production,
            AnalysisRights.AnalysisCertificateCreate)
            )
            .ToProperty(this, e => e.ProductionMode);

        _tests = this.WhenAnyValue(e => e.Model)
            .WhereNotNull()
            .Select(GetTests)
            .ToProperty(this, e => e.Tests);

        _movements = this.WhenAnyValue(e => e.Model)
            .WhereNotNull()
            .Select(GetMovements)
            .ToProperty(this, e => e.Movements);

        _forms = this.WhenAnyValue(e => e.Model)
            .WhereNotNull()
            .Select(GetFormsListViewModel)
            .ToProperty(this, e => e.Forms);

        this.WhenAnyValue(e => e.Model.Customer, e => e.Locker.IsActive)
            .Select(async _ => await GetOrigins())
            .Subscribe();

        this.WhenAnyValue(e => e.Model.Product, e => e.Locker.IsActive)
            .Select(async _ => await UpdateProductLists())
            .Subscribe();

        _workflow = this.WhenAnyValue(e => e.Model, e => e.Locker, selector : (model, locker) =>
        {
            if (model == null || locker == null) return null;
            return _getSampleWorkflow(model, locker);
        }
        ).ToProperty(this , e => e.Workflow);

        CertificateCommand = ReactiveCommand.Create(() => Certificate(false), 
            this.WhenAnyValue(e => e.Injected.Acl)
                .Select(e => e.IsGranted(AnalysisRights.AnalysisCertificateCreate)));

        PreviewCertificateCommand = ReactiveCommand.Create(() => Certificate(true));
    }

    public string SubTitle => _subTitle.Value;
    readonly ObservableAsPropertyHelper<string> _subTitle;

    public bool IsReadOnly => _isReadOnly.Value;
    readonly ObservableAsPropertyHelper<bool> _isReadOnly;

    // Audit Trail
    public SampleAuditTrailViewModel AuditTrail => _auditTrail.Value;
    readonly ObservableAsPropertyHelper<SampleAuditTrailViewModel> _auditTrail;

    public bool AuditDetail
    {
        get => _auditDetail;
        set => this.SetAndRaise(ref _auditDetail, value);
    }
    bool _auditDetail;

    static void OnAuditDetail(bool auditDetail, SampleAuditTrailViewModel auditTrail)
    {
        if (auditDetail)
            auditTrail.List.RemoveFilter("Detail");
        else
            auditTrail.List.AddFilter(e => e.Motivation != null || e.Log.Contains("Stage=") || e.Log.Contains("StageId="), 0, "Detail");

        auditTrail.List.Update();
    }


    public bool CustomerVisibility => _customerVisibility.Value;
    readonly ObservableAsPropertyHelper<bool> _customerVisibility;

    public bool EditMode => _editMode.Value;
    readonly ObservableAsPropertyHelper<bool> _editMode;

    public bool MonographMode => _monographMode.Value;
    readonly ObservableAsPropertyHelper<bool> _monographMode;

    public bool ProductionMode => _productionMode.Value;
    readonly ObservableAsPropertyHelper<bool> _productionMode;

    bool IsStageActive(bool isActive, Workflow<SampleWorkflow>.Stage stage, Workflow<SampleWorkflow>.Stage targetStage, AclRight right)
        => isActive
           && stage == targetStage
           && Injected.Acl.IsGranted(right);

    public SampleSampleTestListViewModel? Tests => _tests.Value;
    readonly ObservableAsPropertyHelper<SampleSampleTestListViewModel?> _tests;

    SampleSampleTestListViewModel? GetTests(Sample sample)
    {
        var tests = _getTests?.Invoke(sample);
        if (tests == null) return tests;

        tests.SetOpenAction(t => _docs.OpenDocumentAsync(t, DocumentPresenter));
        _docs.OpenDocumentAsync(tests, DocumentPresenter);

        return tests;
    }

    public SampleMovementsListViewModel? Movements => _movements.Value;
    readonly ObservableAsPropertyHelper<SampleMovementsListViewModel?> _movements;

    SampleMovementsListViewModel? GetMovements(Sample sample)
    {
        var movements = _getMovements?.Invoke(sample);
        return movements;
    }


    protected override void BeforeSaving(Sample entity)
    {
        base.BeforeSaving(entity);
        Tests.UpdateConformity();
    }

    public SampleFormsListViewModel? Forms => _forms.Value;
    ObservableAsPropertyHelper<SampleFormsListViewModel?> _forms;

    SampleFormsListViewModel? GetFormsListViewModel(Sample sample)
    {
        var forms = _getForms?.Invoke(sample);
        return forms;
    }

    public ObservableCollection<string> CommercialNames { get; } = [];
    public ObservableCollection<string> Origins { get; } = [];
    public ObservableCollection<string> PrimaryPackagingList { get; } = [];
    public ObservableCollection<string> SecondaryPackagingList { get; } = [];
    public ObservableCollection<string> StorageConditionsList { get; } = [];



    async Task GetOrigins()
    {
        if (!Locker.IsActive) return;
        if (Model.Stage != SampleWorkflow.Reception) return;

        var list = await Injected.Data.SelectDistinctAsync<Sample, string>(s => s.CustomerId == Model.CustomerId /*&& !string.IsNullOrWhiteSpace(s.SamplingOrigin)*/,
            s => s.SamplingOrigin).ToListAsync();
        Origins.Clear();

        foreach (var s in list.Where(e => !string.IsNullOrWhiteSpace(e)).OrderBy(e => e))
        {
            Origins.Add(s);
        }
    }

    async Task UpdateProductLists()
    {
        if (!Locker.IsActive) return;
        if (Model.Stage != SampleWorkflow.Reception) return;

        var commercialNamesList = await Injected.Data.SelectDistinctAsync<Sample, string>(s => s.ProductId == Model.ProductId /*&& !string.IsNullOrWhiteSpace(s.SamplingOrigin)*/,
            s => s.CommercialName).ToListAsync();
        CommercialNames.Clear();

        foreach (var s in commercialNamesList.Where(e => !string.IsNullOrWhiteSpace(e)).OrderBy(e => e))
        {
            CommercialNames.Add(s);
        }

        var primaryList = await Injected.Data.SelectDistinctAsync<Sample, string>(s => s.ProductId == Model.ProductId /*&& !string.IsNullOrWhiteSpace(s.SamplingOrigin)*/,
            s => s.PrimaryPackaging).ToListAsync();
        PrimaryPackagingList.Clear();

        foreach (var s in primaryList.Where(e => !string.IsNullOrWhiteSpace(e)).OrderBy(e => e))
        {
            PrimaryPackagingList.Add(s);
        }

        var secondaryList = await Injected.Data.SelectDistinctAsync<Sample, string>(s => s.ProductId == Model.ProductId /*&& !string.IsNullOrWhiteSpace(s.SamplingOrigin)*/,
            s => s.SecondaryPackaging).ToListAsync();
        SecondaryPackagingList.Clear();

        foreach (var s in secondaryList.Where(e => !string.IsNullOrWhiteSpace(e)).OrderBy(e => e))
        {
            SecondaryPackagingList.Add(s);
        }

        var storageList = await Injected.Data.SelectDistinctAsync<Sample, string>(s => s.ProductId == Model.ProductId /*&& !string.IsNullOrWhiteSpace(s.SamplingOrigin)*/,
            s => s.StorageConditions).ToListAsync();
        StorageConditionsList.Clear();

        foreach (var s in storageList.Where(e => !string.IsNullOrWhiteSpace(e)).OrderBy(e => e))
        {
            StorageConditionsList.Add(s);
        }
    }

    public SampleWorkflow Workflow => _workflow.Value;
    ObservableAsPropertyHelper<SampleWorkflow> _workflow;

    public ICommand CertificateCommand { get; }
    public ICommand PreviewCertificateCommand { get; }

    void Certificate(bool preview)
    {
        preview = preview || 
            ( !(Injected.Acl.IsGranted(AnalysisRights.AnalysisCertificateCreate)
                        &&
                        (Model.Stage == SampleWorkflow.Closed || Model.Stage == SampleWorkflow.Certificate)));

        PrintCertificate("FR", preview);
    }

    //    = H.Command(c => c
    //    //.CanExecute(e => e._acl.IsGranted(AnalysisRights.AnalysisCertificateCreate))
    //    .Action(e =>
    //    {
    //    }).CheckCanExecute()
    //);


    void PrintCertificate(string language, bool preview = false)
    {
      // TODO : Implement the logic to print or preview the certificate
        // This could involve generating a PDF or using a reporting tool
        // For now, we will just log the action
        var date = DateTime.Now;
        var formattedDate = DateToString(language, date);
        Console.WriteLine($"Printing certificate for sample {Model.Id} in {language} on {formattedDate}. Preview: {preview}");
   }

    static string DateToString(string language, DateTime date)
    {
        return (language == "EN" || language == "US") ? date.ToString("MM/dd/yyyy") : date.ToString("dd/MM/yyyy");
    }

}