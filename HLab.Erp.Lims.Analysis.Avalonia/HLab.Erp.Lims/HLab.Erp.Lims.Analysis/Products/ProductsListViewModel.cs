using System;
using HLab.Erp.Acl;
using HLab.Erp.Core;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Core.Wpf.EntityLists;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Wpf.Products.ViewModels;

public class ProductsListViewModel(IAclService acl, EntityListViewModel<Product>.Injector i)
    : EntityListViewModel<Product>(i, c => c
        .ColumnListable(e => e.Category)
        .OrderBy(e => e.Category?.Name)
        .Column("Name")
        .Header("{Name}")
        .IconPath("Icons/Entities/Inn")
        .Width(300)
        .Localize(e => e.Name)
        .Link(e => e.Name)
        .OrderBy(e => e.Name)
        .OrderByAsc()
        .Filter()
        .Column("Variant")
        .Header("{Dose}")
        .IconPath("Icons/Entities/Products/Dose")
        .Width(200)
        .Localize(e => e.Variant)
        .Link(e => e.Variant)
        .OrderBy(e => e.Variant)
        .OrderByAsc(1)
        .Filter()
        .ColumnListable(e => e.Form)
        .AddProperty("IsValid", p => true, p => true)), IMvvmContextProvider
{
    public class Bootloader : ParamBootloader;

    //.Column("Category")
    //.Header("{Category}")
    //.Width(100)
    //.Content(e => e.Category.Name).Localize()
    //.FormColumn(e)

    protected override bool AddCanExecute(Action<string> errorAction) => acl.IsGranted(errorAction, AnalysisRights.AnalysisProductCreate);
    protected override bool DeleteCanExecute(Product product, Action<string> errorAction) => acl.IsGranted(errorAction, AnalysisRights.AnalysisProductCreate);
    protected override bool ImportCanExecute(Action<string> errorAction) => acl.IsGranted(errorAction, AnalysisRights.AnalysisProductCreate);
    protected override bool ExportCanExecute(Action<string> errorAction) => true;

    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }
}