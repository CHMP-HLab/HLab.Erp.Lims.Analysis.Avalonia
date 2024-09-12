using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Products;

public class InnViewModelDesign() : InnViewModel(null, p => null), IDesignViewModel
{
    public new Inn Model { get; } = Inn.DesignModel;
}