using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Data.Entities;

namespace HLab.Erp.Lims.Analysis.Wpf.Pharmacopoeias;

public class PharmacopoeiaViewModel(EntityViewModel<Pharmacopoeia>.Injector i)
    : ListableEntityViewModel<Pharmacopoeia>(i);