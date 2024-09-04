using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data;

public partial class Pharmacopoeia : Entity, IListableModel, ILocalCache
{
    public Pharmacopoeia() => _caption = this.WhenAnyValue(e => e.Name).ToProperty(this, e => e.Caption);

    public string Name
    {
        get => _name;
        set => SetAndRaise(ref _name, value);
    }
    private string _name = "";

    public string Abbreviation
    {
        get => _abbreviation;
        set => SetAndRaise(ref _abbreviation, value);
    }
    private string _abbreviation = "";


    public string Url
    {
        get => _url;
        set => SetAndRaise(ref _url, value);
    }
    private string _url = "";


    public string SearchUrl
    {
        get => _searchUrl;
        set => SetAndRaise(ref _searchUrl, value);
    }
    private string _searchUrl = "";


    public string ReferenceUrl
    {
        get => _referenceUrl;
        set => SetAndRaise(ref _referenceUrl, value);
    }
    private string _referenceUrl = "";

    public string IconPath
    {
        get => _iconPath;
        set => SetAndRaise(ref _iconPath, value);
    }
    private string _iconPath = "";

    [Ignore]
    public string Caption => _caption.Value;
    ObservableAsPropertyHelper<string> _caption;


    public static Pharmacopoeia DesignModel => new Pharmacopoeia
    {
        Name = "{US Pharmacopoeia}",
        Abbreviation = "Usp"
    };

}

