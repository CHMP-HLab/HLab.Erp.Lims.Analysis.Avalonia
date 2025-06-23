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

    public string Name { get; set => this.SetAndRaise(ref field, value); } = "";

    public string EnglishName { get; set => this.SetAndRaise(ref field, value); } = "";

    public string IconPath { get; set => this.SetAndRaise(ref field, value); } = "";

    [Ignore]
    public string Caption => _caption.Value;
    readonly ObservableAsPropertyHelper<string> _caption;
}
