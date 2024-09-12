using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Data.Entities;

namespace HLab.Erp.Lims.Analysis.Products;

public class ProductCategoryViewModel(EntityViewModel<ProductCategory>.Injector i)
    : ListableEntityViewModel<ProductCategory>(i);