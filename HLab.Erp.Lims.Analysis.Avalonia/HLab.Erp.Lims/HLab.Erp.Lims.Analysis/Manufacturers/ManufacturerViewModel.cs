using HLab.Erp.Acl;
using HLab.Erp.Base.Customers;
using HLab.Erp.Base.Wpf.Entities.Customers;
using HLab.Erp.Lims.Analysis.Data.Entities;

namespace HLab.Erp.Lims.Analysis.Manufacturers;

public class ManufacturerViewModel(EntityViewModel<Manufacturer>.Injector i) : CorporationViewModel<Manufacturer>(i);