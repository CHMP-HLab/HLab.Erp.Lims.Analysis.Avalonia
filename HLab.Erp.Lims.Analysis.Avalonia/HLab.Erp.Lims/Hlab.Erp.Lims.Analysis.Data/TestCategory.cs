using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;

namespace HLab.Erp.Lims.Analysis.Data;

public class TestCategory : Entity, ILocalCache, IListableModel
{
    public TestCategory() { }

    public static TestCategory DesignModel => new TestCategory
    {
        Name = "Design Category",
        Priority = 1,
        IconPath = "Icons/Default"
    };

    public string Name
    {
        get => _name;
        set => SetAndRaise(ref _name,value);
    }
    private string _name = "";
    public int? Priority
    {
        get => _priority;
        set => SetAndRaise(ref _priority,value);
    }
    private int? _priority ;
    [Ignore]
    public string Caption => Name;
    public string IconPath
    {
        get => _iconPath;
        set => SetAndRaise(ref _iconPath,value);
    }
    private string _iconPath = "";
}