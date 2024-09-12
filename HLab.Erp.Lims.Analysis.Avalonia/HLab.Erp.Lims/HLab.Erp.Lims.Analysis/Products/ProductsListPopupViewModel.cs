using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Extensions;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Products;

public class ProductsListPopupViewModel(EntityListViewModel<Product>.Injector i)
    : EntityListViewModel<Product>(i, c => c
        .Column("Name")
        .Header("{Name}")
        .Width(200)
        .Link(p => p.Name)
        .Filter()
        .Column("Variant")
        .Header("{Variant}")
        .Width(100)
        .Link(p => p.Variant)
        .Filter()
        .FormColumn(p => p.Form)), IMvvmContextProvider
{
    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }
}