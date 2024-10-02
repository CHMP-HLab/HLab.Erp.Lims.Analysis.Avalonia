using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;
using System.Reactive.Linq;
using HLab.Base.ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public class TestCategory : Entity, ILocalCache, IListableModel
{
    public TestCategory() {
        _caption = this
            .WhenAnyValue(e => e.Name)
            .IfNullOrWhiteSpace("{New TestCategory}")
            .Select(name => $"{{Test category}}\n{name}")
            .ToProperty(this, e => e.Caption);
    }

    public static TestCategory DesignModel => new TestCategory
    {
        Name = "Design Category",
        Priority = 1,
        IconPath = "Icons/Default"
    };

    public string Name
    {
        get => _name;
        set => this.SetAndRaise(ref _name, value);
    }

    string _name = "";
    public int? Priority
    {
        get => _priority;
        set => this.SetAndRaise(ref _priority, value);
    }

    int? _priority;
    public string IconPath
    {
        get => _iconPath;
        set => this.SetAndRaise(ref _iconPath, value);
    }

    string _iconPath = "";

    [Ignore] public string Caption => _caption.Value;
    readonly ObservableAsPropertyHelper<string> _caption;


}