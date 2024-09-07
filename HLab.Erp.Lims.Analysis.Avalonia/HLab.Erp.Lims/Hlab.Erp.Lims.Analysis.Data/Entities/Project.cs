using HLab.Erp.Data;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public class Project : Entity
{
    public Project()
    {
        _parent = Foreign(this, e => e.ParentId, e => e.Parent);
    }

    public int? ParentId
    {
        get => _parent.Id;
        set => _parent.SetId(value);
    }

    public Project Parent
    {
        get => _parent.Value;
        set => ParentId = value.Id;
    }
    readonly ForeignPropertyHelper<Project, Project> _parent;

    public string Name
    {
        get => _name;
        set => SetAndRaise(ref _name, value);
    }
    private string _name = "";

}
