using System.Reactive.Linq;
using System.Windows.Input;
using HLab.Base.Extensions;
using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Erp.Lims.Analysis.FormClasses;
using HLab.Erp.Lims.Analysis.Samples.SampleMovements;
using HLab.Mvvm.Annotations;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Samples.SampleTests.SampleTestResults;

public class SampleTestResultViewModel : EntityViewModel<SampleTestResult>
{
    public class Design :  SampleTestResultViewModel, IDesignViewModel {}
    SampleTestResultViewModel():base(null) {}

    readonly Func<Sample, IDataLocker<Sample>> _getSampleLocker;
    readonly Func<SampleTest, IDataLocker<SampleTest>> _getSampleTestLocker;

    public SampleTestResultViewModel(
        Injector i,
        Func<FormHelper?> getFormHelper, 
        Func<int, SampleTestResultAuditTrailViewModel> getAudit,
        Func<SampleTestResult, IDataLocker<SampleTestResult>, SampleTestResultWorkflow> getWorkflow,
        Func<SampleTestResult, LinkedDocumentsListViewModel> getDocuments,
        Func<SampleTestResult, SampleMovementsListViewModel> getMovements,
        Func<Sample, IDataLocker<Sample>> getSampleLocker,
        Func<SampleTest, IDataLocker<SampleTest>> getSampleTestLocker
    ):base(i)
    {
        _getAudit = getAudit;
        _getWorkflow = getWorkflow;
        _getDocuments = getDocuments;
        _getMovements = getMovements;
        _getSampleLocker = getSampleLocker;
        _getSampleTestLocker = getSampleTestLocker;

        _auditTrail = this.WhenAnyValue(e => e.Model)
            .Select(e => _getAudit?.Invoke(e.Id))
            .ToProperty(this, e => e.AuditTrail);

        this.WhenAnyValue(e => e.AuditDetail, e => e.AuditTrail)
            .Subscribe(e => OnAuditDetailChanged(e.Item1,e.Item2));

        this.WhenAnyValue(e => e.Model, e => e.Locker)
            .Subscribe(e => OnModelChanged(e.Item1, e.Item2));

        this.WhenAnyValue(e => e.Model, e => e.Locker)
            .Subscribe(e => OnModelChanged(e.Item1, e.Item2));

        _workflow = this.WhenAnyValue(
            e => e.Model, 
            e => e.Locker, 
            selector: (sample,locker) => _getWorkflow?.Invoke(sample, locker)
            )
            .ToProperty(this, e => e.Workflow);

        _editMode = this.WhenAnyValue(
            e => e.Injected.Acl, 
            e => e.Locker.IsActive, 
            e => e.Workflow.CurrentStage, 
            e => e.Model.SampleTest.Stage, 
            e => e.Model.SampleTest.Sample.Stage, 
            GetEditMode)
            .ToProperty(this, e => e.EditMode);

        FormHelper = getFormHelper?.Invoke();

        //readonly ITrigger _ = H.Trigger(c => c
        //    .On(e => e.Model.Stage)
        //    //            .On(e => e.Model.Values)
        //    .On(e => e.EditMode)
        //    .OnNotNull(e => e.Workflow)
        //    .Do(async e => await e.LoadResultAsync())
        //);

        this.WhenAnyValue(e => e.Model)
            .Select(async e => await LoadResultAsync(e))
            .Subscribe();

        _movements = this.WhenAnyValue(e => e.Model)
            .Select(e => _getMovements?.Invoke(e))
            .ToProperty(this, e => e.Movements);

        _linkedDocuments = this.WhenAnyValue(e => e.Model)
            .Select(e => _getDocuments?.Invoke(e).FluentAction(vm => vm.SetOpenAction(d => LinkedDocuments.Selected.OpenDocument())))
            .ToProperty(this, e => e.LinkedDocuments);

        PrintCommand = ReactiveCommand.Create(Print
            //,this.WhenAnyValue(e => e.Workflow.CurrentStage));
            );

        _header = this
            .WhenAnyValue(
            e => e.Model.SampleTest.Sample.Reference, 
            e => e.Model.Name,
            selector : (reference,name) => $"{reference} - {name}"
                )
            .ToProperty(this, e => e.Header);

        _subTitle = this
            .WhenAnyValue(
            e => e.Model.SampleTest.TestName,
            e => e.Model.SampleTest.Description,
            selector : (testName, description) => $"{testName}\n{description.TrimEnd('\r', '\n', ' ')}"
            )
            .ToProperty(this, e => e.SubTitle);

        OpenSampleCommand = ReactiveCommand.CreateFromTask(async () => await Injected.Docs.OpenDocumentAsync(Model.SampleTest.Sample));

        OpenProductCommand = ReactiveCommand.CreateFromTask(async () => await Injected.Docs.OpenDocumentAsync(Model.SampleTest.Sample.Product));

        OpenCustomerCommand = ReactiveCommand.CreateFromTask(async () => await Injected.Docs.OpenDocumentAsync(Model.SampleTest.Sample.Customer));

        OpenTestCommand = ReactiveCommand.CreateFromTask(async () => await Injected.Docs.OpenDocumentAsync(Model.SampleTest));
    }

    // Audit Trail
    readonly Func<int, SampleTestResultAuditTrailViewModel> _getAudit;
    public SampleTestResultAuditTrailViewModel AuditTrail => _auditTrail.Value;
    readonly ObservableAsPropertyHelper<SampleTestResultAuditTrailViewModel> _auditTrail;


    public bool AuditDetail
    {
        get => _auditDetail;
        set => SetAndRaise(ref _auditDetail,value);
    }
    bool _auditDetail ;

    static void OnAuditDetailChanged( bool auditDetail, SampleTestResultAuditTrailViewModel auditTrail)
    {
        if (auditDetail)
            auditTrail.List.RemoveFilter("Detail");
        else
            auditTrail.List.AddFilter(e => e.Motivation != null || e.Log.Contains("Stage=") || e.Log.Contains("StageId="), 0, "Detail");
        auditTrail.List.Update();
    }

    void OnModelChanged(SampleTestResult? model, IDataLocker<SampleTestResult> locker)
    {
        locker.AddDependencyLocker(
                    _getSampleLocker(model.SampleTest.Sample),
                    _getSampleTestLocker(model.SampleTest));
    }

    readonly Func<SampleTestResult, IDataLocker<SampleTestResult>, SampleTestResultWorkflow> _getWorkflow;

    public SampleTestResultWorkflow Workflow => _workflow.Value;
    readonly ObservableAsPropertyHelper<SampleTestResultWorkflow> _workflow;


    public bool EditMode => _editMode.Value;
    readonly ObservableAsPropertyHelper<bool> _editMode;

    bool GetEditMode(IAclService acl, bool isActive, 
        SampleTestResultWorkflow.Stage resultStage, 
        SampleTestWorkflow.Stage testStage, 
        SampleWorkflow.Stage sampleStage
        )
    {
        return isActive
               && resultStage == SampleTestResultWorkflow.Running
               && testStage == SampleTestWorkflow.Running
               && sampleStage == SampleWorkflow.Production
               && acl.IsGranted(AnalysisRights.AnalysisResultEnter);
    }

    public SampleTestViewModel Parent
    {
        get => _parent;
        set => SetAndRaise(ref _parent,value);
    }

    SampleTestViewModel _parent ;


    public FormHelper? FormHelper {get;}

    public async Task LoadResultAsync(SampleTestResult result)
    {
        await FormHelper.LoadAsync(result).ConfigureAwait(true);
        FormHelper.Form.Mode = Workflow.CurrentStage == SampleTestResultWorkflow.Running ? FormMode.Capture : FormMode.ReadOnly;
    }


    // Stock Movements
    readonly Func<SampleTestResult, SampleMovementsListViewModel> _getMovements;
    public SampleMovementsListViewModel Movements => _movements.Value;
    readonly ObservableAsPropertyHelper<SampleMovementsListViewModel> _movements;

    // LINKED DOCUMENTS
    readonly Func<SampleTestResult, LinkedDocumentsListViewModel> _getDocuments;

    public LinkedDocumentsListViewModel LinkedDocuments => _linkedDocuments.Value;
    readonly ObservableAsPropertyHelper<LinkedDocumentsListViewModel> _linkedDocuments;

    public ICommand PrintCommand { get; } 

    // TODO : URGENT
    void Print()
    {
        /*
        var printDialog = new PrintDialog();
        if (printDialog.ShowDialog() != true) return;

        if (FormHelper.Form is not Control visual) return;

        var f = visual.Foreground;
        var b = visual.Background;

        visual.Foreground = Brushes.Black;
        visual.Background = Brushes.White;

        var dic = new ResourceDictionary { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Themes/light.blue.xaml") };

        visual.Resources.MergedDictionaries.Add(dic);

        printDialog.PrintVisual( visual, "My First Print Job");

        visual.Foreground = f;
        visual.Background = b;

        visual.Resources.MergedDictionaries.Remove(dic);
        */
    }

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////
    /// </summary>
    /// 
    public override string Header => _header.Value;

    readonly ObservableAsPropertyHelper<string> _header;

    public string SubTitle => _subTitle.Value;
    readonly ObservableAsPropertyHelper<string> _subTitle;

    public ICommand OpenSampleCommand { get; } 
    public ICommand OpenProductCommand { get; }
    public ICommand OpenCustomerCommand { get; }
    public ICommand OpenTestCommand { get; }
}