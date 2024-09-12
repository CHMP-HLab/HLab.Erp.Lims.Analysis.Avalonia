using HLab.Erp.Core;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Products;

public class ProductFormsListViewModel(EntityListViewModel<Form>.Injector i)
    : Core.EntityLists.EntityListViewModel<Form>(i, c => c
        .Column("Name")
        .Header("{Name}")
        .Link(e => e.Name)
        .OrderByAsc(0)
        .Filter()
        .Column("Icon")
        .Header("{Icon}")
        .Icon((s) => s.IconPath)
        .OrderBy(s => s.Name)), IMvvmContextProvider
{
    public class Bootloader : ParamBootloader 
    {        
        public override string MenuPath => "param/products";
    }

    protected override bool AddCanExecute(Action<string> errorAction) => true;
    protected override bool DeleteCanExecute(Form form, Action<string> errorAction) => true;

    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }

}