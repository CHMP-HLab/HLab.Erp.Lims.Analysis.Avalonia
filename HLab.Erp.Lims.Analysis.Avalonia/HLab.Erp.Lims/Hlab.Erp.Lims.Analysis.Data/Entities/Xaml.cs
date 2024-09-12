using HLab.Erp.Data;

namespace HLab.Erp.Lims.Analysis.Data.Entities;
public class Xaml : Entity, ILocalCache
{
    public Xaml() { }

    public string Name
    {
        get => _name;
        set => SetAndRaise(ref _name, value);
    }

    string _name = "";
    public string Page
    {
        get => _page;
        set => SetAndRaise(ref _page, value);
    }

    string _page = "";
}
