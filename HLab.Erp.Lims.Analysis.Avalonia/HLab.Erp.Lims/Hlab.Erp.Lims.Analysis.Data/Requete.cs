using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;

namespace HLab.Erp.Lims.Analysis.Data;

public partial class Requete : Entity, IListableModel, ILocalCache
{
    public Requete() { }

    public override string ToString() => Nom;

    public string Nom
    {
        get => _nom; set => SetAndRaise(ref _nom,value);
    }
    private string _nom = "";
    public string P1
    {
        get => _p1; set => SetAndRaise(ref _p1,value);
    }
    private string _p1 = "";
    public string P2
    {
        get => _p2; set => SetAndRaise(ref _p2,value);
    }
    private string _p2 = "";
    public string P3
    {
        get => _p3; set => SetAndRaise(ref _p3,value);
    }
    private string _p3 = "";
    public string P4
    {
        get => _p4; set => SetAndRaise(ref _p4,value);
    }
    private string _p4 = "";
    public string T1
    {
        get => _t1; set => SetAndRaise(ref _t1,value);
    }
    private string _t1 = "";
    public string T2
    {
        get => _t2; set => SetAndRaise(ref _p2,value);
    }
    private string _t2 = "";
    public string T3
    {
        get => _t3; set => SetAndRaise(ref _t3,value);
    }
    private string _t3 = "";
    public string T4
    {
        get => _t4; set => SetAndRaise(ref _t4,value);
    }
    private string _t4 = "";
    [Column("Requete")]
    public string Query
    {
        get => _query; set => SetAndRaise(ref _query,value);
    }
    private string _query = "";
    public string Parametres
    {
        get => _parametres; set => SetAndRaise(ref _parametres,value);
    }
    private string _parametres = "";
    public string TaillesColonnes
    {
        get => _taillesColonnes; set => SetAndRaise(ref _taillesColonnes,value);
    }
    private string _taillesColonnes = "";
    public string Droit
    {
        get => _droit; set => SetAndRaise(ref _droit,value);
    }
    private string _droit = "";
    public string Cache
    {
        get => _cache; set => SetAndRaise(ref _cache,value);
    }
    private string _cache = "";

    [Ignore]
    public string Caption => "";

    [Ignore]
    public string IconPath => "";

}