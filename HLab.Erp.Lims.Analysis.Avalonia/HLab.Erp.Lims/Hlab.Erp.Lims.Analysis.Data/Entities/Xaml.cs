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

    private string _name = "";
    public string Page
    {
        get => _page;
        set => SetAndRaise(ref _page, value);
    }

    private string _page = "";
}
