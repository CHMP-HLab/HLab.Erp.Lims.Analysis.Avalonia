using System.Reactive.Linq;
using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Wpf.Products.ViewModels;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Products;

public class ProductViewModel: ListableEntityViewModel<Product>
{
    public ProductViewModel(Injector i, Func<Product, ProductProductComponentsListViewModel> getComponents):base(i)
    {
        _getComponents = getComponents;

        _subTitle = this.WhenAnyValue(
            e => e.Model.Variant,
            e => e.Model.Form.Name,
            GetSubTitle)
            .ToProperty(this, e => e.SubTitle);

        _iconPath = this.WhenAnyValue(
            e => e.Model.Form.IconPath,
            e => e.Model.IconPath,
            selector: (formIcon,selfIcon) => formIcon??selfIcon??"")
            .ToProperty(this, e => e.IconPath);

        this.WhenAnyValue(e => e.Locker.IsActive)
            .Do(lockerIsActive =>
            {
                if(Components!=null)
                    Components.EditMode = lockerIsActive;
            }).Subscribe();

        _components = this.WhenAnyValue(e => e.Model, selector: _getComponents)
            .ToProperty(this, e => e.Components);
    }
    public string SubTitle => _subTitle.Value;
    readonly ObservableAsPropertyHelper<string> _subTitle;

    static string GetSubTitle(string variant, string form) => $"{variant}\n{form}";


    public override string IconPath => _iconPath.Value;
    readonly ObservableAsPropertyHelper<string> _iconPath;

    string GetIconPath() => Model?.Form?.IconPath??Model?.IconPath??base.IconPath;


    readonly Func<Product, ProductProductComponentsListViewModel> _getComponents;

    public ProductProductComponentsListViewModel Components => _components.Value;
    readonly ObservableAsPropertyHelper<ProductProductComponentsListViewModel> _components;



    //public ProductWorkflow Workflow => _workflow.Get();
    //private ProductWorkflow _workflow = H.Property<ProductWorkflow>(c => c
    //    .On(e => e.Model)
    //    .OnNotNull(e => e.Locker)
    //    .Set(vm => new ProductWorkflow(vm.Model,vm.Locker))
    //);
}