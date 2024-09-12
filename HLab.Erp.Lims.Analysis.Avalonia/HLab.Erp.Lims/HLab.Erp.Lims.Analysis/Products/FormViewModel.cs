using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Data.Entities;

namespace HLab.Erp.Lims.Analysis.Products;

public class FormViewModel(EntityViewModel<Form>.Injector i) : ListableEntityViewModel<Form>(i);