using HLab.Erp.Acl;
using HLab.Erp.Core;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Manufacturers;

public class ManufacturersListViewModel(IAclService acl, EntityListViewModel<Manufacturer>.Injector i)
    : EntityListViewModel<Manufacturer>(i, c => c
        .Column("Name")
        .Header("{Name}")
        .Width(250)
        .OrderByAsc(0)
        .Link(e => e.Name)
        .Filter()
        .Header("{Name}")
        .ColumnListable(e => e.Country, "Country").Width(150)), IMvvmContextProvider
{
    public class Bootloader : NestedBootloader
    { }

    protected override bool AddCanExecute(Action<string> errorAction) => acl.IsGranted(errorAction, AnalysisRights.AnalysisManufacturerCreate);
    protected override bool DeleteCanExecute(Manufacturer manufacturer, Action<string> errorAction) => acl.IsGranted(errorAction, AnalysisRights.AnalysisManufacturerCreate);

    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }

}