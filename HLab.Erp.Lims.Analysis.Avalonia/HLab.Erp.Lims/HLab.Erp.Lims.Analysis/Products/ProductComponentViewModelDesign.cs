using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Products;

public class ProductComponentViewModelDesign() : ProductComponentViewModel(null), IDesignViewModel
{
    public new ProductComponent Model { get; } = ProductComponent.DesignModel;

}