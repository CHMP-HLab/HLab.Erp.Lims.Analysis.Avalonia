using System.ComponentModel.DataAnnotations.Schema;
using HLab.Erp.Data;

namespace HLab.Erp.Lims.Analysis.Data;

public class Packaging : Entity, ILocalCache
{
    public Packaging() { }

    public bool Secondary
    {
        get => _secondary;
        set => SetAndRaise(ref _secondary,value);
    }
    private bool _secondary ;


    [Column("Nom")]
    public string Name
    {
        get => _name;
        set => SetAndRaise(ref _name,value);
    }
    private string _name ;

}