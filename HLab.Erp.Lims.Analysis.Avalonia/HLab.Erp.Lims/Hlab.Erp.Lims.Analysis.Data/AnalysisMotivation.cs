using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data;

public partial class AnalysisMotivation : Entity, IListableModel, ILocalCache
{
    public static AnalysisMotivation DesignModel => new() { Name="My Form"};

    public AnalysisMotivation() 
    {
        _caption = this.WhenAnyValue(e => e.Name)
            .ToProperty(this, e => e.Caption);
    }

    public override string ToString() => Name;


    public string Name
    {
        get => _name; set => SetAndRaise(ref _name,value);
    }
    string _name = "";


    public string IconPath
    {
        get => _iconPath;
        set => SetAndRaise(ref _iconPath,value);
    }
    string _iconPath = "";

    [Ignore]
    public string Caption => _caption.Value;
    readonly ObservableAsPropertyHelper<string> _caption;

}
