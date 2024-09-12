using HLab.Erp.Core;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Products;

public class InnsListViewModel(EntityListViewModel<Inn>.Injector i) : Core.EntityLists.EntityListViewModel<Inn>(i, c =>
    c
        .Column("Name")
        .Header("{Name}")
        .IconPath("Icons/Entities/Inn")
        .Width(250)
        .Localize(e => e.Name)
        .Icon(p => p.IconPath)
        .Link(e => e.Name)
        .OrderByAsc(0)
        .Filter()), IMvvmContextProvider
{
    public class Bootloader : ParamBootloader { }

    protected override bool AddCanExecute(Action<string> errorAction) => true;
    protected override bool DeleteCanExecute(Inn inn, Action<string> errorAction) => true;

    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }

}