using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;
using System.Reactive.Linq;
using HLab.Base.ReactiveUI;

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

    public string Name { get; set => this.SetAndRaise(ref field, value); } = "";

    public string NamePropertyName { get; set => this.SetAndRaise(ref field, value); } = "{Name}";

    public string VariantPropertyName { get; set => this.SetAndRaise(ref field, value); } = "{Variant}";

    public string ComplementPropertyName { get; set => this.SetAndRaise(ref field, value); } = "{Complement}";

    public int? Priority { get; set => this.SetAndRaise(ref field, value); }

    [Ignore]
    public string Caption => _caption.Value;
    readonly ObservableAsPropertyHelper<string> _caption;

    public string IconPath { get; set => this.SetAndRaise(ref field, value); } = "";
}