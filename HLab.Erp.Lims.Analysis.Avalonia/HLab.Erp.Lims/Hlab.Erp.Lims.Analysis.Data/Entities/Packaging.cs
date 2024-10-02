using System.ComponentModel.DataAnnotations.Schema;
using HLab.Base.ReactiveUI;
using HLab.Erp.Data;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public class Packaging : Entity, ILocalCache
{
    public Packaging() { }

    public bool Secondary
    {
        get => _secondary;
        set => this.SetAndRaise(ref _secondary, value);
    }

    bool _secondary;


    [Column("Nom")]
    public string Name
    {
        get => _name;
        set => this.SetAndRaise(ref _name, value);
    }

    string _name;

}