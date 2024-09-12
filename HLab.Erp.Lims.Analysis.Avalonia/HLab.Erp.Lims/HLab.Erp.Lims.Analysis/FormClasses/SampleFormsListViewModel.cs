using HLab.Erp.Acl;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.FormClasses;

public class SampleFormsListViewModel(
    IDataService data,
    IAclService acl,
    EntityListViewModel<SampleForm>.Injector i,
    Sample sample)
    : Core.EntityLists.EntityListViewModel<SampleForm>(i, c => c
        .List<SampleFormsListViewModel>(out var list)
        .StaticFilter(e => e.SampleId == sample.Id)
        .Column("Name")
        .Header("{Name}")
        .Width(200)
        .Content(s => s.FormClass.Name)
        .OrderBy(s => s.FormClass?.Name)
        .OrderByAsc(0)
        .Icon(s => s.FormClass.IconPath)), IMvvmContextProvider
{
    readonly IDataService _data = data;

    public Sample Sample {get; } = sample;

    // TODO : we need to trigger add command can execute when the stage changes
    //readonly ITrigger _ = H.Trigger(c => c
    //    .On(e => e.Sample.Stage)
    //    .On(e => e.Sample.Id)
    //    .Do(e => (e.AddCommand as CommandPropertyHolder)?.CheckCanExecute())
    //);

    protected override bool AddCanExecute(Action<string> errorAction)
    {
        if (Sample.Id < 0) {
            errorAction("{Please save before adding forms}");
            return false;
        };
            
        var stage = Sample.Stage.IsAny(errorAction,SampleWorkflow.Reception);
        var granted = acl.IsGranted(errorAction,AnalysisRights.AnalysisReceptionSign);
        return stage && granted;
    }

    public override Type AddArgumentClass => typeof(FormClass);


    static void Add(SampleFormsListViewModel list, SampleForm sampleForm, object arg)
    {
        if (!(arg is FormClass formClass)) return;

        //var exists = await Erp.Data.FetchOneAsync<SampleForm>(sf => sf.SampleId == Sample.Id && sf.FormClassId == formClass.Id);
        //if(exists == null)
        foreach(var sf in list.List)
        {
            if(sf.SampleId == list.Sample.Id && sf.FormClassId == formClass.Id)
            {
                return;
            }
        }

        sampleForm.Sample = list.Sample;
        sampleForm.FormClass = formClass;
    }

    protected override bool DeleteCanExecute(SampleForm target, Action<string> errorAction)
    {
        if (target?.Sample == null) return false;
        var stage = target.Sample.Stage.IsAny(errorAction, SampleWorkflow.Reception);
        var granted =acl.IsGranted(errorAction, AnalysisRights.AnalysisReceptionCreate);
        return stage && granted;
    }

    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }
}