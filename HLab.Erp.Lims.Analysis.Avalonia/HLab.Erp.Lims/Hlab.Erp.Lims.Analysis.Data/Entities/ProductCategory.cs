using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;
using System.Reactive.Linq;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public class ProductCategory : Entity, ILocalCache, IListableModel
{
    public static TestCategory DesignModel => new TestCategory
    {
        Name = "Design Category",
        Priority = 1,
        IconPath = "Icons/Default"
    };

    public ProductCategory() {

        _caption = this
            .WhenAnyValue(e => e.Name)
            .IfNullOrWhiteSpace("{New product category}")
            .Select(name => $"{{Product category}}\n{name}")
            .ToProperty(this, e => e.Caption);
    }

    public string Name
    {
        get => _name;
        set => SetAndRaise(ref _name, value);
    }

    string _name = "";

    public string NamePropertyName
    {
        get => _namePropertyName;
        set => SetAndRaise(ref _namePropertyName,value);
    }

    string _namePropertyName = "{Name}";

    public string VariantPropertyName
    {
        get => _variantPropertyName;
        set => SetAndRaise(ref _variantPropertyName,value);
    }

    string _variantPropertyName = "{Variant}";

    public string ComplementPropertyName
    {
        get => _complementPropertyName;
        set => SetAndRaise(ref _complementPropertyName,value);
    }

    string _complementPropertyName = "{Complement}";
    public int? Priority
    {
        get => _priority;
        set => SetAndRaise(ref _priority, value);
    }

    int? _priority;

    #region IListableModel
    [Ignore]
    public string Caption => _caption.Value;
    readonly ObservableAsPropertyHelper<string> _caption;

    public string IconPath
    {
        get => _iconPath;
        set => SetAndRaise(ref _iconPath, value);
    }

    string _iconPath = "";
    #endregion
}