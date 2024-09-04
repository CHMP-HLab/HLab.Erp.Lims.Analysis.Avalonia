using HLab.Erp.Base.Data;
using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data;

public partial class Manufacturer : Corporation, ILocalCache, IListableModel
{

    public Manufacturer()
    {
        _caption = this.WhenAnyValue(e => e.Id, e => e.Name, selector: GetCaption).ToProperty(this, e => e.Caption);
        _iconPath = this.WhenAnyValue(e => e.Country.IconPath).ToProperty(this, e => e.IconPath);
    }
    [Ignore]
    public string Caption => _caption.Value;
    ObservableAsPropertyHelper<string> _caption;
    static string GetCaption(int id, string name) => (id < 0 && string.IsNullOrEmpty(name)) ? "Nouveau client" : name;

    [Ignore] public string IconPath => _iconPath.Value;
    ObservableAsPropertyHelper<string> _iconPath;

}
