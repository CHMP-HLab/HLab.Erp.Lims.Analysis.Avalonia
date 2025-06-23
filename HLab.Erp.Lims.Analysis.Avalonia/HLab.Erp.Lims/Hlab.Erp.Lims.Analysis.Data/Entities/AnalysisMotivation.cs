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

    public string Name { get; set => this.SetAndRaise(ref field, value); } = "";

    public string IconPath { get; set => this.SetAndRaise(ref field, value); } = "";

    [Ignore]
    public string Caption => _caption.Value;
    readonly ObservableAsPropertyHelper<string> _caption;
}
