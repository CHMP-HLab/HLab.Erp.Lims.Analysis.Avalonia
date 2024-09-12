using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Products;

public class ProductViewModelDesign() : ProductViewModel(null, p => null), IDesignViewModel
{
    public new Product Model { get; } = Product.DesignModel;

}