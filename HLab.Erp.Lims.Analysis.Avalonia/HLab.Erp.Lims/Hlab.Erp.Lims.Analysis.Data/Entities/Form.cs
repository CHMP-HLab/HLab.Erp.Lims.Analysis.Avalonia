using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;
using System.Reactive.Linq;
using HLab.Base.ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public partial class Form : Entity, IListableModel, ILocalCache
{
    public static Form DesignModel => new() { Name = "Tablet" };

    public override string ToString() => Name;

    public Form()
    {

        _caption = this
            .WhenAnyValue(f => f.Name)
            .Select(name => string.IsNullOrWhiteSpace(name) ? "{New product form}" : name)
            .ToProperty(this, f => f.Caption);
    }

    public string Name
    {
        get => _name;
        set => this.SetAndRaise(ref _name, value);
    }
    string _name = "";

    public string EnglishName
    {
        get => _englishName;
        set => this.SetAndRaise(ref _englishName, value);
    }

    string _englishName = "";

    public string IconPath
    {
        get => _iconPath;
        set => this.SetAndRaise(ref _iconPath, value);
    }
    string _iconPath = "";

    [Ignore]
    public string Caption => _caption.Value;
    ObservableAsPropertyHelper<string> _caption;
}
