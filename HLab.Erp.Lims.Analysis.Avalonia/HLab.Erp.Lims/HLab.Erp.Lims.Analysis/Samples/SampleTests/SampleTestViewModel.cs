using System.Reactive.Linq;
using System.Windows.Input;
using HLab.Base.ReactiveUI;
using HLab.Erp.Acl;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Erp.Lims.Analysis.FormClasses;
using HLab.Erp.Lims.Analysis.Wpf.Samples.SampleTests;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Samples.SampleTests;

public class SampleTestViewModelDesign()
    : SampleTestViewModel(null, null, null, null, null, null, null, null), IDesignViewModel;

public class SampleTestViewModel : EntityViewModel<SampleTest>, IMvvmContextProvider
{
    public IDocumentPresenter DocumentPresenter { get; }
    public IDocumentService Docs { get; }
    readonly Func<Sample, DataLocker<Sample>> _getSampleLocker;
    readonly Func<SampleTest,TestResultsListViewModel> _getResults;
    readonly Func<SampleTest, IDataLocker<SampleTest>, SampleTestWorkflow> _getSampleTestWorkflow;
    readonly Func<FormHelper> _getFormHelper;
    readonly Func<int, SampleTestAuditTrailViewModel> _getAudit;

    public SampleTestViewModel(
        Injector i,
        IDocumentService docs,
        IDocumentPresenter presenter,
        Func<SampleTest, TestResultsListViewModel> getResults,
        Func<int, SampleTestAuditTrailViewModel> getAudit,
        Func<FormHelper> getFormHelper,
        Func<SampleTest, IDataLocker<SampleTest>, SampleTestWorkflow> getSampleTestWorkflow,
        Func<Sample, DataLocker<Sample>> getSampleLocker
    ):base(i)
    {
        DocumentPresenter = presenter;
        Docs = docs;
        _getResults = getResults;
        _getAudit = getAudit;
        _getFormHelper = getFormHelper;
        _getSampleTestWorkflow = getSampleTestWorkflow;
        _getSampleLocker = getSampleLocker;

        this.WhenAnyValue(
            e => e.Locker,
            e => e.Model.Sample 
            )
            .Select(v =>
            {
                var (locker, sample) = v;
                locker.AddDependencyLocker(_getSampleLocker(sample));
                return locker.IsActive;
            })
            .Subscribe();
            
        FormHelper = _getFormHelper?.Invoke();

        _auditTrail = this.WhenAnyValue(e => e.Model)
            .Select(e => _getAudit?.Invoke(e.Id))
            .ToProperty(this, e => e.AuditTrail);

        this.WhenAnyValue(e => e.AuditDetail, e => e.AuditTrail)
            .Subscribe(e => OnAuditDetailChanged(e.Item1, e.Item2));

        _workflow = this.WhenAnyValue(
            e => e.Model, 
            e => e.Locker,
            selector : (model, locker) => _getSampleTestWorkflow?.Invoke(model,locker)
        
        ).ToProperty(this, e => e.Workflow);
        
        _editMode = this.WhenAnyValue(
            e => e.FormHelper.Form,
            e => e.Injected.Acl,
            e => e.Locker.IsActive,
            e => e.Workflow.CurrentStage,
            GetEditMode
        ).ToProperty(this, e => e.EditMode);

        _scheduleEditMode = this.WhenAnyValue(
            e => e.FormHelper.Form,
            e => e.Injected.Acl,
            e => e.Locker.IsActive,
            e => e.Workflow.CurrentStage,
            GetScheduleEditMode
        ).ToProperty(this, e => e.ScheduleEditMode);

        this.WhenAnyValue(
            e => e.EditMode,
            e => e.ResultMode,
            (editMode, resultMode) => editMode || resultMode
        ).ToProperty(this, e => e.FormHelperIsActive);

        _results = this.WhenAnyValue(e => e.Model)
            .Select(e => GetResults())
            .ToProperty(this, e => e.Results);

        this.WhenAnyValue(
            e => e.Model.Result,
            e => e.Results.Selected,
            selector : (result, selected) => selected ?? result
            )
            .Select(async e => await LoadResultAsync(e))
            .Subscribe();

        ViewSpecificationsCommand = ReactiveCommand.CreateFromTask(async () => await LoadResultAsync());

        AddResultCommand = ReactiveCommand.Create(
            () => AddResult(Results.Selected),
            this.WhenAnyValue(
                e => e.Injected.Acl,
                e => e.Workflow.CurrentStage,
                selector: _addResultCanExecute
                )
            );

        DeleteResultCommand = ReactiveCommand.Create(
            () => DeleteResult(Results.Selected),
            this.WhenAnyValue(
                e => e.Workflow.CurrentStage,
                e => e.Results.Selected.Stage,
                e => e.Results.Selected,
                e => e.Model.Result
                ).Select(w => _deleteResultCanExecute())
            );

        SelectResultCommand = ReactiveCommand.CreateFromTask(
            async () => await SelectResult(Locker.IsActive,Results.Selected,Model.Stage),
            this.WhenAnyValue(
                e => e.Locker.IsActive,
                e => e.Results.Selected.Stage,
                e => e.Model.Stage,
                SelectCanExecute
            ));

        OpenSampleCommand = ReactiveCommand.CreateFromTask(
            async () => await Docs.OpenDocumentAsync(Model.Sample)
        );

        OpenProductCommand = ReactiveCommand.CreateFromTask(
            async () => await Docs.OpenDocumentAsync(Model.Sample.Product)
        );

        OpenCustomerCommand = ReactiveCommand.CreateFromTask(
            async () => await Docs.OpenDocumentAsync(Model.Sample.Customer)
        );

        _testHelper = this.WhenAnyValue(e => e.FormHelper.Form.Target)
            .ToProperty(this, e => e.TestHelper);

        _iconPath = this.WhenAnyValue(e => e.Model.IconPath)
            .ToProperty(this, e => e.IconPath);

        _header = this.WhenAnyValue(e => e.Model.TestName)
            .ToProperty(this, e => e.Header);

        _subTitle = this.WhenAnyValue(e => e.Model.Description)
            .Select(e => e.TrimEnd('\r', '\n', ' '))
            .ToProperty(this, e => e.SubTitle);
    }

    public FormHelper FormHelper {get;}

    public async Task LoadResultAsync(IFormTarget target=null)
    {
        await FormHelper.LoadAsync(target??Model).ConfigureAwait(true);

        var state = Workflow.CurrentStage;

        if (state == SampleTestWorkflow.Specifications) 
            FormHelper.Form.Mode = FormMode.Specification;
        //else if (state == SampleTestWorkflow.Running) 
        //    FormHelper.Mode = TestFormMode.Capture;
        else 
            FormHelper.Form.Mode = FormMode.ReadOnly;
    }

    // Audit Trail
    public SampleTestAuditTrailViewModel AuditTrail => _auditTrail.Value;
    ObservableAsPropertyHelper<SampleTestAuditTrailViewModel> _auditTrail;


    public bool AuditDetail
    {
        get => _auditDetail;
        set => this.SetAndRaise(ref _auditDetail,value);
    }
    bool _auditDetail ;

    void OnAuditDetailChanged(bool auditDetail, SampleTestAuditTrailViewModel auditTrail)
    {
        if (auditDetail)
            auditTrail.List.RemoveFilter("Detail");
        else
            auditTrail.List.AddFilter(e => e.Motivation != null || e.Log.Contains("Stage=") || e.Log.Contains("StageId="), 0, "Detail");
        auditTrail.List.Update();
    }


    public SampleTestWorkflow Workflow => _workflow.Value;
    readonly ObservableAsPropertyHelper<SampleTestWorkflow> _workflow;


    public bool EditMode => _editMode.Value;
    ObservableAsPropertyHelper<bool> _editMode;

    bool GetEditMode(IForm form, IAclService acl, bool lockerIsActive, SampleTestWorkflow.Stage testStage)
    {
        form.SetFormMode(FormHelper.Form.Mode);

        return lockerIsActive
               && testStage == SampleTestWorkflow.Specifications
               && acl.IsGranted(AnalysisRights.AnalysisMonographSign);
    }



    public bool ScheduleEditMode => _scheduleEditMode.Value;
    readonly ObservableAsPropertyHelper<bool> _scheduleEditMode;

    static bool GetScheduleEditMode(IForm form, IAclService acl, bool lockerIsActive, SampleTestWorkflow.Stage testStage)
    {
        return lockerIsActive
               && testStage == SampleTestWorkflow.Scheduling
               && acl.IsGranted(AnalysisRights.AnalysisSchedule);
    }


    public bool ResultMode => _resultMode.Value;
    readonly ObservableAsPropertyHelper<bool> _resultMode;
    static bool GetResultMode(IForm form, IAclService acl, bool lockerIsActive, SampleTestWorkflow.Stage testStage)
    {
        return lockerIsActive
               && testStage == SampleTestWorkflow.Running
               && acl.IsGranted(AnalysisRights.AnalysisResultEnter);
    }


    public bool FormHelperIsActive => _formHelperIsActive.Value;
    readonly ObservableAsPropertyHelper<bool> _formHelperIsActive;




    // RESULTS
    public TestResultsListViewModel Results => _results.Value;
    readonly ObservableAsPropertyHelper<TestResultsListViewModel> _results;


    TestResultsListViewModel GetResults()
    {
        var results =  _getResults(Model);

        if (results != null)
        {
            results.SetOpenAction(t => Docs.OpenDocumentAsync(t,DocumentPresenter));
            Docs.OpenDocumentAsync(results, DocumentPresenter);

            results.SetSelectAction(async r =>
            {
                await LoadResultAsync(r as SampleTestResult).ConfigureAwait(false);
                // TODO : if (SelectResultCommand is ReactiveCommand nc) nc.CheckCanExecute();
            });

            SampleTestResult selected = null;
            foreach(var result in results.List)
            {
                if(selected==null) selected = result;
                if(result == Model.Result) selected = result;
            }
            results.Selected = selected;
        }

        return results;
    }

    public ICommand ViewSpecificationsCommand { get; }
    public ICommand AddResultCommand { get; }
    public ICommand DeleteResultCommand { get; }

    static bool _addResultCanExecute(IAclService acl, SampleTestWorkflow.Stage stage)
    {
        if(!acl.IsGranted(AnalysisRights.AnalysisAddResult)) return false;
        if(stage != SampleTestWorkflow.Running) return false;

        return true;
    }

    bool _deleteResultCanExecute()
    {
        if (Workflow == null) return false;
        if (Results?.Selected == null) return false;
        if(!Injected.Acl.IsGranted(AnalysisRights.AnalysisAddResult)) return false;
        if(Workflow.CurrentStage != SampleTestWorkflow.Running) return false;
        if (Results.Selected.Stage != null && Results.Selected.Stage != SampleTestResultWorkflow.Running) return false;
        if (Model.Result == null) return true;
        if(Model.Result.Id == Results.Selected.Id) return false;
        return true;
    }


    public ICommand SelectResultCommand { get; }
        
    async Task SelectResult(bool lockerIsActive, SampleTestResult? result, SampleTestWorkflow.Stage testStage)
    {
        if (result != null && SelectCanExecute(lockerIsActive, result.Stage, testStage))
             await SelectResult(result);
    }

    static bool SelectCanExecute(bool lockerIsActive, SampleTestResultWorkflow.Stage? resultStage, SampleTestWorkflow.Stage? testStage)
    {
        return resultStage == SampleTestResultWorkflow.Validated
               && testStage == SampleTestWorkflow.Running
               && lockerIsActive;
    }


    public ICommand OpenSampleCommand { get; }
    public ICommand OpenProductCommand { get; }
    public ICommand OpenCustomerCommand { get; }

    async Task SelectResult(SampleTestResult result)
    {
        if(result.Stage == SampleTestResultWorkflow.Validated)
        {
            Model.Result = result;
            await Results.List.RefreshAsync();
        }
    }

    void AddResult(SampleTestResult previous)
    {
        int i = 0;

        foreach (var r in Results.List)
        {
            var n = r.Name??"";
            if (n.StartsWith("R")) n = n.Substring(1);

            if(int.TryParse(n, out var v))
            {
                i = Math.Max(i,v);
            }
        }            
            
        var test = Injected.Data.Add<SampleTestResult>(r =>
        {
            r.Name = string.Format("R{0}",i+1);
            r.SampleTest = Model;
            r.UserId = Model.UserId;
        });

        if (test != null)
            Results.List.Update();
    }

    void DeleteResult(SampleTestResult result)
    {
        if(_deleteResultCanExecute())
        {
            Injected.Data.Delete(result);

        }
    }


    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////
    /// </summary>

    public IFormTarget TestHelper => _testHelper.Value;

    readonly ObservableAsPropertyHelper<IFormTarget> _testHelper;

    public override string IconPath => _iconPath.Value;
    readonly ObservableAsPropertyHelper<string> _iconPath;

    public override string Header => _header.Value;
    readonly ObservableAsPropertyHelper<string> _header;

    public string SubTitle => _subTitle.Value;
    readonly ObservableAsPropertyHelper<string> _subTitle;



    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
//            ctx.AddCreator<SampleTestResultViewModel>(vm => vm.Parent = this);
    }
}