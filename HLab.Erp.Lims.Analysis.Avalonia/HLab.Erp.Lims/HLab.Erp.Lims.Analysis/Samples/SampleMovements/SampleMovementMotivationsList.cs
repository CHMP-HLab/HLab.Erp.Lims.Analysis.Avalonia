using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Lims.Analysis.Data.Entities;

namespace HLab.Erp.Lims.Analysis.Wpf.Samples.SampleMovements;

public class SampleMovementMotivationsList(EntityListViewModel<SampleMovementMotivation>.Injector injector)
    : EntityListViewModel<SampleMovementMotivation>(injector, c => c
        .Column("Motivation")
        .Header("{Motivation}")
        .Link(s => s.Name)
        .Filter());