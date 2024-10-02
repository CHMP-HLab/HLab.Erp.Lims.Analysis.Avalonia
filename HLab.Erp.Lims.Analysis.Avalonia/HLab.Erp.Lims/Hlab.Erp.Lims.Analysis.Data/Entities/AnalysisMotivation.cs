using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;
using System.Reactive.Linq;
using HLab.Base.ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public partial class AnalysisMotivation : Entity, IListableModel, ILocalCache
{
    public static AnalysisMotivation DesignModel => new() { Name = "My Form" };

    public AnalysisMotivation()
    {
        _caption = this
            .WhenAnyValue(e => e.Name)
            .Select(name => string.IsNullOrWhiteSpace(name) ? "{New motivation}" : $"{{Motivation}}\n{name}")
            .ToProperty(this, e => e.Caption);
    }

    public override string ToString() => Name;


    public string Name
    {
        get => _name; set => this.SetAndRaise(ref _name, value);
    }
    string _name = "";


    public string IconPath
    {
        get => _iconPath;
        set => this.SetAndRaise(ref _iconPath, value);
    }
    string _iconPath = "";

    [Ignore]
    public string Caption => _caption.Value;
    readonly ObservableAsPropertyHelper<string> _caption;

}
