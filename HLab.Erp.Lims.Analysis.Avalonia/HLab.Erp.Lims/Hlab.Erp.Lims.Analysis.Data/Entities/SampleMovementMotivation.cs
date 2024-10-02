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

    public string Name
    {
        get => _name;
        set => this.SetAndRaise(ref _name,value);
    }
    string _name = "";

    public string IconPath
    {
        get => _iconPath;
        set => this.SetAndRaise(ref _iconPath,value);
    }
    string _iconPath = "";

    [Ignore] public string Caption => _caption.Value;
    readonly ObservableAsPropertyHelper<string> _caption;

}