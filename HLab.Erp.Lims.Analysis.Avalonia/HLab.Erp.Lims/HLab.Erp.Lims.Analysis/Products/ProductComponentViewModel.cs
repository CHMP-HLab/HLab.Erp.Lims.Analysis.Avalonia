using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Data.Entities;

namespace HLab.Erp.Lims.Analysis.Products;

public class ProductComponentViewModel(EntityViewModel<ProductComponent>.Injector i)
    : ListableEntityViewModel<ProductComponent>(i)
{

    //public string SubTitle => _subTitle.Get();
    //private string _subTitle = H.Property<string>(c => c
    //    .Set(e => e.GetSubTitle )
    //    .On(e => e.Model.Variant)
    //    .On(e => e.Model.Form.Name)
    //    .Update()
    //);
    //private string GetSubTitle => $"{Model?.Variant}\n{Model?.Form?.Name}";


    public override string IconPath => "";//_iconPath.Get();
    //private string _iconPath = H.Property<string>(c => c
    //.Set(e => e.GetIconPath )
    //.On(e => e.Model.Form.IconPath)
    //.On(e => e.Model.IconPath)
    //.Update()
    //);

    //private string GetIconPath => Model?.Form?.IconPath??Model?.IconPath??base.IconPath;


    //public ProductWorkflow Workflow => _workflow.Get();
    //private ProductWorkflow _workflow = H.Property<ProductWorkflow>(c => c
    //    .On(e => e.Model)
    //    .OnNotNull(e => e.Locker)
    //    .Set(vm => new ProductWorkflow(vm.Model,vm.Locker))
    //);
}