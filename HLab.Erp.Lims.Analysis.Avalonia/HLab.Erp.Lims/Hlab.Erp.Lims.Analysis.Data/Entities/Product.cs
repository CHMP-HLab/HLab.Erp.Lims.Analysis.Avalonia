using HLab.Base.ReactiveUI;
using HLab.Erp.Data;
using HLab.Erp.Data.foreigners;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public partial class Product : Entity, ILocalCache, IListableModel
{
    public Product()
    {
        _form = this.Foreign( e => e.FormId, e => e.Form);

        _category = this.Foreign( e => e.CategoryId, e => e.Category);

        _caption = this.WhenAnyValue(
            e => e.Name,
            e => e.Variant,
            e => e.Form,
            (name, variant, form) => $"{name} - {form?.Caption??""} ({variant})")
            .ToProperty(this, e => e.Caption);

        _iconPath = this.WhenAnyValue(e => e.Form.IconPath)
            .ToProperty(this, e => e.IconPath);
    }

    public string Name { get; set => this.SetAndRaise(ref field, value);} = "";

    public string Variant { get; set => this.SetAndRaise(ref field, value); } = "";

    public string Complement { get; set => this.SetAndRaise(ref field, value); } = "";

    public string Note { get;  set => this.SetAndRaise(ref field, value); } = "";

    [Ignore] public string Caption => _caption.Value;
    readonly ObservableAsPropertyHelper<string> _caption;

    [Ignore] public string IconPath => _iconPath.Value;
    readonly ObservableAsPropertyHelper<string> _iconPath;

    public int? FormId { get => _form.Id; set => _form.SetId(value); }
    [Ignore] public Form Form { get => _form.Value; set => FormId = value.Id; }
    readonly ForeignPropertyHelper<Product, Form> _form;

    public int? CategoryId { get => _category.Id; set => _category.SetId(value); }
    [Ignore] public ProductCategory Category { get => _category.Value; set => CategoryId = value.Id; }
    readonly ForeignPropertyHelper<Product, ProductCategory> _category;

    public static Product DesignModel => new Product
    {
        Name = "Paracetamol",
        Variant = "20 mg",
        Note = "Design time model"
    };
}
