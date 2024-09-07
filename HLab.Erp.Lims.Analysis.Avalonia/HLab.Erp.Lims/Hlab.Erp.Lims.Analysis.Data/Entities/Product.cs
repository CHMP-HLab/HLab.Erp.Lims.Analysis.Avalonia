using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public partial class Product : Entity, ILocalCache, IListableModel
{
    public Product()
    {
        _caption = this.WhenAnyValue(
            e => e.Name,
            e => e.Variant,
            e => e.Form,
            (name, variant, form) => $"{name} - {form.Caption} ({variant})")
            .ToProperty(this, e => e.Caption);

        _iconPath = this.WhenAnyValue(e => e.Form.IconPath)
            .ToProperty(this, e => e.IconPath);

        _form = Foreign(this, e => e.FormId, e => e.Form);
        _category = Foreign(this, e => e.CategoryId, e => e.Category);

    }

    public string Name
    {
        get => _name;
        set => SetAndRaise(ref _name,value);
    }

    string _name = "";


    public string Variant
    {
        get => _variant;
        set => SetAndRaise(ref _variant,value);
    }

    string _variant = "";


    public string Complement
    {
        get => _complement;
        set => SetAndRaise(ref _complement, value);
    }
    string _complement = "";


    public string Note
    {
        get => _note;
        set => SetAndRaise(ref _note, value);
    }
    string _note = "";

    [Ignore]
    public string Caption => _caption.Value;
    readonly ObservableAsPropertyHelper<string> _caption;

    [Ignore]
    public string IconPath => _iconPath.Value;
    readonly ObservableAsPropertyHelper<string> _iconPath;

    public int? FormId
    {
        get => _form.Id;
        set => _form.SetId(value);
    }
    [Ignore]
    public Form Form
    {
        get => _form.Value;
        set => FormId = value.Id;
    }
    readonly ForeignPropertyHelper<Product, Form> _form;

    public int? CategoryId
    {
        get => _category.Id;
        set => _category.SetId(value);
    }
    [Ignore]
    public ProductCategory Category
    {
        get => _category.Value;
        set => CategoryId = value.Id;
    }
    readonly ForeignPropertyHelper<Product, ProductCategory> _category;


    public static Product DesignModel => new Product
    {
        Name = "Paracetamol",
        Variant = "20 mg",
        Note = "Design time model"
    };
}
