using HLab.Erp.Acl;
using HLab.Erp.Core;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.FormClasses;

public class FormClassesListViewModel(IAclService acl, EntityListViewModel<FormClass>.Injector i)
    : EntityListViewModel<FormClass>(i, c => c
        .Column("Name")
        .Header("{Name}")
        .Width(200)
        .Content(s => s.Name)
        .OrderBy(s => s.Name)
        .OrderByAsc(0)
        .Icon(s => s.IconPath)
        .Column("Class")
        .Header("{Class}")
        .Width(100).Content(s => s.Class ?? "")
        .OrderBy(s => s.Class)), IMvvmContextProvider
{
    public class Bootloader : NestedBootloader
    {
        public override string MenuPath => "param";
    }

    protected override bool AddCanExecute(Action<string> errorAction)
    {
        return acl.IsGranted(errorAction, AnalysisRights.AnalysisProductCreate);
    }

    protected override bool DeleteCanExecute(FormClass target,Action<string> errorAction)
    {
        RemoveErrorMessage("Delete");
        return acl.IsGranted(errorAction, AnalysisRights.AnalysisProductCreate);
    }

    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }
}