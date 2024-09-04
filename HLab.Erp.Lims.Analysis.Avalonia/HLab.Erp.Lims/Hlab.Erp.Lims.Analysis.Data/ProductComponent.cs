using HLab.Erp.Data;
using NPoco;

namespace HLab.Erp.Lims.Analysis.Data;



public class ProductComponent : Entity
{
    public ProductComponent() {
        _parent = Foreign(this, e => e.ParentId, e => e.Parent);
        _child = Foreign(this, e => e.ChildId, e => e.Child);
    }

    public int? ParentId
    {
        get => _parent.Id;
        set => _parent.SetId(value);
    }

    [Ignore]
    public Product Parent
    {
        set => ParentId = value.Id;
        get => _parent.Value;
    }
    readonly ForeignPropertyHelper<ProductComponent, Product> _parent;

    public int? ChildId
    {
        get => _child.Id;
        set => _child.SetId(value);
    }

    [Ignore]
    public Product Child
    {
        get => _child.Value;
        set => ChildId = value.Id;
    }
    readonly ForeignPropertyHelper<ProductComponent, Product> _child;
}