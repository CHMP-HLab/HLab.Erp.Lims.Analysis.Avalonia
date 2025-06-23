using HLab.Base.ReactiveUI;
using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public class SampleMovementMotivation : Entity, IListableModel
{
    public SampleMovementMotivation()
    {
        _caption = this
            .WhenAnyValue(e => e.Name)
            .IfNullOrWhiteSpace("{New SampleMovementMotivation}")
            .ToProperty(this, e => e.Caption);
    }

    public string Name { get; set => this.SetAndRaise(ref field, value); } = "";

    public string IconPath { get; set => this.SetAndRaise(ref field, value); } = "";

    [Ignore] public string Caption => _caption.Value;
    readonly ObservableAsPropertyHelper<string> _caption;

}