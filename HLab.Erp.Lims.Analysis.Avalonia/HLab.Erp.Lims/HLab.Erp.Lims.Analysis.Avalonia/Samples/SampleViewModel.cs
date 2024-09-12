using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using HLab.Erp.Acl;
using HLab.Erp.Base;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
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
        set => SetAndRaise(ref _auditDetail, value);
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


    void PrintCertificate(String language, bool preview = false)
    {
        if (Model.Id == -1)
            return;

        var template = Injected.Data.FetchOne<Xaml>(e => e.Name == "Certificate");

        // Prépare l'impression
        var ip = new Print("Certificate", template.Page, language);

        var expiry = "";
        if (Model.ExpirationDate != null)
        {
            expiry = Model.ExpirationDate?.ToString(!Model.ExpirationDayValid ? "MM/yyyy" : "dd/MM/yyyy");
        }
        ip["ExpirationDate"] = expiry;

        var manufacturingDate = "";
        if (Model.ManufacturingDate != null)
        {
            manufacturingDate = Model.ManufacturingDate?.ToString(!Model.ManufacturingDayValid ? "MM/yyyy" : "dd/MM/yyyy");
        }
        ip["ManufacturingDate"] = manufacturingDate;


        if (Model.Validator != null)
        {
            ip["Validator.Caption"] = $"DR {Model.Validator.Caption}";
            ip["Validator.Function"] = Model.Validator.Function;
        }
        else
        {
            ip["Validator.Caption"] = $"Analyse non validée";
            ip["Validator.Function"] = "";
        }

        ip.SetData(Model);

        // Cache le bandeau d'aperçu
        if (!preview)
            ip.Cache("Apercu");

        // Ajout des tests sur la page
        var nomTest = "";

        bool? conform = true;

        var startDate = DateTime.MaxValue;
        var endDate = DateTime.MinValue;


        foreach (var test in Tests.List)
        {
            if (test.Stage != SampleTestWorkflow.InvalidatedResults)
            {
                var testStartDate = test.StartDate ?? test.Result?.Start ?? DateTime.MaxValue;
                var testEndDate = test.EndDate ?? test.Result?.End ?? DateTime.MinValue;

                if (testStartDate > testEndDate) testStartDate = testEndDate;
                if (testEndDate < testStartDate) testEndDate = testStartDate;

                if (testStartDate < startDate) startDate = testStartDate;
                if (testEndDate > endDate) endDate = testEndDate;

                // Ajoute la ligne pour le nom du test
                //if (test.TestName != nomTest)
                //{
                //    nomTest = test.TestName;
                //    ip.AjouteElement("Titre");
                //    ip.Element["Titre"] = " " + nomTest;
                //}

                // Les résultats du test
                ip.AjouteElement();

                // si même nom de test ne pas repeter
                if (nomTest == test.TestName)
                {
                    ip.Element.ReplaceZone("Titre", "");
                }
                else
                {
                    nomTest = test.TestName;
                    ip.Element["Titre"] = " " + nomTest;
                }

                ip.Element["Date"] = testEndDate == DateTime.MinValue
                    ? "__/ __ /_____"
                    : language is "US" or "EN"
                        ? testEndDate.ToString("MM/dd/yyyy")
                        : testEndDate.ToString("dd/MM/yyyy");

                ip.Element["Description"] = test.Description + Environment.NewLine;
                ip.Element["Reference"] = test.Pharmacopoeia?.Abbreviation ?? "" + " " + test.PharmacopoeiaVersion + Environment.NewLine;
                ip.Element["Specification"] = test.Specification + Environment.NewLine;
                ip.Element["Result"] = test.Result?.Result ?? "" + Environment.NewLine;

                switch (test.Result?.ConformityId)
                {
                    case ConformityState.NotConform:
                        ip.Element["Conform"] = "{FR=Non conforme}{EN=Not conform}" + Environment.NewLine;
                        conform = false;
                        break;
                    case ConformityState.Conform:
                        ip.Element["Conform"] = "{FR=Conforme}{EN=Conform}" + Environment.NewLine;
                        break;
                    case ConformityState.Invalid:
                        ip.Element["Conform"] = "{FR=Invalide}{EN=Invalid}" + Environment.NewLine;
                        conform = null;
                        break;
                    case ConformityState.NotChecked:
                        ip.Element["Conform"] = "{FR=Non testé}{EN=Not tested}" + Environment.NewLine;
                        conform = null;
                        break;
                    case ConformityState.Running:
                        ip.Element["Conform"] = "{FR=En cours}{EN=Running}" + Environment.NewLine;
                        conform = null;
                        break;
                    case null:
                        ip.Element["Conform"] = "{FR=Non validé}{EN=Not validated}" + Environment.NewLine;
                        conform = null;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        ip["Conformity"] = (conform == true) ? "Conforme" : (conform == false ? "Non conforme" : "Indeterminée");
        ip["XConform"] = (conform == true) ? "X" : " ";
        ip["XNotConform"] = (conform == false) ? "X" : " ";

        ip["AnalysisStart"] = DateToString(language, startDate);
        ip["AnalysisEnd"] = DateToString(language, endDate);

        if (string.IsNullOrWhiteSpace(Model.Conclusion))
        {
            ip.ReplaceZone("Conclusion", "");
        }

        // Impression du certificat d'analyse
        if (ip.Apercu("Rapport_" + Model.Reference, null, Print.Langue("{FR=Rapport d'analyse}{EN=Report of analysis} ", language) + Model.Reference))
        {
            // Log cette impression
            // TODO : Sql.Log(TypeObjet.Echantillon, IdEchantillon, ip.LogText);
        }

    }

    static string DateToString(string language, DateTime date)
    {
        return (language == "EN" || language == "US") ? date.ToString("MM/dd/yyyy") : date.ToString("dd/MM/yyyy");
    }

}