using HLab.Erp.Base.Data;
using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;

namespace HLab.Erp.Lims.Analysis.Data;

public partial class Manufacturer : Corporation, ILocalCache, IListableModel
{

    public Manufacturer()
    {
        _caption = this.WhenAnyValue(e => e.Name, selector: ).ToProperty(this, e => e.Caption);
        _iconPath = this.WhenAnyValue(e => e.Country.IconPath).ToProperty(this, e => e.IconPath);
    }
    [Ignore]
    public string Caption => _caption.Get();
    private string _caption = H.Property<string>(c => c
        .On(e => e.Name)
        .On(e => e.Id)
        //TODO : localize
        .Set(e => (e.Id < 0 && string.IsNullOrEmpty(e.Name)) ? "Nouveau client" : e.Name)
    );

    [Ignore] public string IconPath => _iconPath.Get();
    private string _iconPath = H.Property<string>(c => c
        .Bind(e => e.Country.IconPath)
    );

}
