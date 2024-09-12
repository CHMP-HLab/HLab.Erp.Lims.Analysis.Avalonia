using HLab.Erp.Core;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Lims.Analysis.Data.Entities;

namespace HLab.Erp.Lims.Analysis.Stats;

public class QueryListViewModel(EntityListViewModel<StatQuery>.Injector i)
    : Core.EntityLists.EntityListViewModel<StatQuery>(i, c => c
        .Column("Name")
        .Header("{Name}")
        .Width(500)
        .Link(s => s.Name)
        .Filter())
{
    public class Bootloader : NestedBootloader
    {
        public override string MenuPath => "tools";
    }
}