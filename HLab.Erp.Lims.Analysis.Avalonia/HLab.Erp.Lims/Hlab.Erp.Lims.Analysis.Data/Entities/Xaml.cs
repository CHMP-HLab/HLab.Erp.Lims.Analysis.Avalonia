using HLab.Base.ReactiveUI;
using HLab.Erp.Data;

namespace HLab.Erp.Lims.Analysis.Data.Entities;
public class Xaml : Entity, ILocalCache
{
    public Xaml() { }

    public string Name
    {
        get => _name;
        set => this.SetAndRaise(ref _name, value);
    }

    string _name = "";
    public string Page
    {
        get => _page;
        set => this.SetAndRaise(ref _page, value);
    }

    string _page = "";
}
