using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using HLab.Base.ReactiveUI;
using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Erp.Lims.Analysis.FormClasses;
using HLab.Erp.Lims.Analysis.Samples;
using HLab.Erp.Lims.Analysis.Wpf.Samples;
using HLab.Mvvm.Annotations;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Wpf.FormClasses;


public class SampleFormViewModelDesign() : SampleFormViewModel(null, null), IDesignViewModel;

public class SampleFormViewModel : ListableEntityViewModel<SampleForm>
{
    public SampleFormViewModel(Injector i, Func<FormHelper> getFormHelper) : base(i)
    {
        
        FormHelper = getFormHelper();

        _editMode = this.WhenAnyValue(
            e => e.Locker.IsActive, 
            e => e.Model.Sample,
            e => e.Injected.Acl,
            selector: (isActive, sample, acl) => 
                isActive 
                && sample.Stage == SampleWorkflow.Reception 
                && acl.IsGranted(AnalysisRights.AnalysisResultEnter)
            )
            .ToProperty(this, e => e.EditMode);

        _conformity = this.WhenAnyValue(e => e.Model.ConformityId)
            .Select(e => e.ToString())
            .ToProperty(this, e => e.Conformity);

        this.WhenAnyValue(e => e.Model.Sample.Stage, e => e.EditMode)
            .Select(async e => await LoadAsync())
            .Subscribe();
    }

    public bool EditMode => _editMode.Value;
    readonly ObservableAsPropertyHelper<bool> _editMode;

    public string Conformity => _conformity.Value;
    readonly ObservableAsPropertyHelper<string> _conformity;

    public SampleViewModel Parent
    {
        get => _parent;
        set => this.SetAndRaise(ref _parent,value);
    }

    SampleViewModel _parent ;

    public FormHelper FormHelper
    {
        get => _formHelper;
        set => this.SetAndRaise(ref _formHelper,value);
    }

    FormHelper _formHelper ;


    //public ITestHelper TestHelper => _testHelper.Get();
    //private ITestHelper _testHelper = H.Property<ITestHelper>(c => c
    //    .Set(e => e.FormHelper?.Form?.Test)
    //    .On(e => e.FormHelper.Form.Test)
    //    //.NotNull(e => e.FormHelper?.Form?.Test)
    //    .Update()
    //);

    public async Task LoadAsync()
    {
        //await FormHelper.ExtractCodeAsync(Model.FormClass.Code).ConfigureAwait(true);

        //await FormHelper.LoadFormAsync(Model).ConfigureAwait(true);

        //FormHelper.Form.LoadValues(Model.SpecificationValues);
        //FormHelper.Form.LoadValues(Model.ResultValues);

        //FormHelper.Form.Mode = Model.Sample.Stage == SampleWorkflow.Reception ? FormMode.Capture : FormMode.ReadOnly;

        await FormHelper.LoadAsync(Model).ConfigureAwait(true);

        FormHelper.Form.Mode = Model.Sample.Stage == SampleWorkflow.Reception ? FormMode.Capture : FormMode.ReadOnly;

    }


    //public string SubTitle => _subTitle.Get();
    //private string _subTitle = H.Property<string>(c => c
    //    .Set(e => e.Model.SampleTest.TestName + "\n" + e.Model.SampleTest.Description)
    //    .On(e => e.Model.SampleTest.TestName)
    //    .On(e => e.Model.SampleTest.Description)
    //    .Update()
    //    );
}