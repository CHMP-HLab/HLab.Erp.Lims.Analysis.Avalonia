using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Core.Wpf.EntityLists;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Wpf.TestClasses;

internal class TestCategoriesListViewModel(EntityListViewModel<TestCategory>.Injector i)
    : EntityListViewModel<TestCategory>(i, c => c.Column("Icon")
        .Width(80)
        .Icon(s => s.IconPath, 30)
        .Column("Name")
        .Header("{Name}")
        .Width(200)
        .Content(s => s.Name)), IMvvmContextProvider
{
    //.AddAllowed()
    //.DeleteAllowed()

    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }
}