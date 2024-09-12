using HLab.Erp.Acl;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Products;


public class InnProductComponentListViewModel(
    IAclService acl,
    IDataService data,
    EntityListViewModel<ProductComponent>.Injector i,
    Inn inn)
    : Core.EntityLists.EntityListViewModel<ProductComponent>(i, c => c.StaticFilter(e => e.InnId == inn.Id)
        .Column("Product")
        .Header("{Product}")
        .Width(100)
        .Content(e => e.Product == null ? "" : e.Product.Name)
        .OrderBy(e => e.Product?.Name)
        .OrderByAsc(0)
        .Column("Dose")
        .Header("{Dose}")
        .Width(100)
        .Content(e => e.Quantity)
        .OrderBy(e => e.Quantity)
        .Column("Unit")
        .Header("{Unit}")
        .Width(100)
        .Content(e => e.Quantity)
        .OrderBy(e => e.Quantity)), IMvvmContextProvider
{
    readonly IDataService _data = data;

    public Inn Inn { get; } = inn;

    //.DeleteAllowed()
    // List.AddOnCreate(h => h.Entity. = "<Nouveau Critère>").Update();

    public bool EditMode { get => _editMode; set => SetAndRaise(ref _editMode,value); }
    bool _editMode = false;

    protected override bool DeleteCanExecute(ProductComponent sampleTest, Action<string> errorAction)
    {
        if (!EditMode) return false;
        if (sampleTest == null) return false;
        //var stage =  sampleTest.Stage.IsAny( errorAction, SampleTestWorkflow.Specifications);
        //var granted = Erp.Acl.IsGranted(errorAction, AnalysisRights.AnalysisAddTest);
        //return stage && granted;
        return true;
    }

    public override Type AddArgumentClass => typeof(Inn);

    // TODO ReactiveUi : we need to trigger add command can execute when the stage changes
    //readonly ITrigger _1 = H.Trigger(c => c
    //    //.On(e => e.Sample.Stage)
    //    //.On(e => e.Sample.Pharmacopoeia)
    //    //.On(e => e.Sample.PharmacopoeiaVersion)
    //    .On(e => e.EditMode)
    //    .Do(e => (e.AddCommand as CommandPropertyHolder)?.CheckCanExecute())
    //);

    //private readonly ITrigger _triggerConformity = H.Trigger(c => c
    //    .On(e => e.List.Item().Result.ConformityId)
    //    .On(e => e.List.Item().Result.Stage)
    //    .On(e => e.List.Item().Result.Start)
    //    .On(e => e.List.Item().Result.End)
    //    .Do(e => e.List.Refresh())
    //);

    protected override bool AddCanExecute(Action<string> errorAction)
    {
        if (!EditMode) return false;
        if (!acl.IsGranted(errorAction, AnalysisRights.AnalysisAddTest)) return false;
        //if (Sample.Pharmacopoeia == null)
        //{
        //    errorAction("{Missing} : {Pharmacopoeia}");
        //    return false;
        //}
        //if (string.IsNullOrWhiteSpace(Sample.PharmacopoeiaVersion))
        //{
        //    errorAction("{Missing} : {Pharmacopoeia version}");
        //    return false;
        //}
        //if (! Sample.Stage.IsAny(errorAction,SampleWorkflow.Monograph))
        //{
        //    errorAction("{requier stage} : {Monograph}");
        //    return false;
        //}
        return true;
    }

    protected override Task ConfigureNewEntityAsync(ProductComponent pc, object arg)
    {
        pc.Inn = Inn;
        return base.ConfigureNewEntityAsync(pc, arg);
    }


    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }
}