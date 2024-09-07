using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;
using System.Reactive.Linq;

namespace HLab.Erp.Lims.Analysis.Data.Entities
{
    public partial class StatQuery : Entity, IListableModel, ILocalCache
    {
        public override string ToString() => Name;

        public StatQuery()
        {
            _caption = this
                .WhenAnyValue(e => e.Name)
                .IfNullOrWhiteSpace("{New query}")
                .Select(name => $"{{Query}}\n{name}")
                .ToProperty(this, e => e.Caption);
        }

        [Column("Nom")]
        public string Name
        {
            get => _name; set => SetAndRaise(ref _name,value);
        }

        string _name = "";
        public string P1
        {
            get => _p1; set => SetAndRaise(ref _p1,value);
        }

        string _p1 = "";
        public string P2
        {
            get => _p2; set => SetAndRaise(ref _p2,value);
        }

        string _p2 = "";
        public string P3
        {
            get => _p3; set => SetAndRaise(ref _p3,value);
        }

        string _p3 = "";
        public string P4
        {
            get => _p4; set => SetAndRaise(ref _p4,value);
        }

        string _p4 = "";
        public int T1
        {
            get => _t1; set => SetAndRaise(ref _t1,value);
        }

        int _t1 ;
        public int T2
        {
            get => _t2; set => SetAndRaise(ref _t2,value);
        }

        int _t2 ;
        public int T3
        {
            get => _t3; set => SetAndRaise(ref _t3,value);
        }

        int _t3 ;
        public int T4
        {
            get => _t4; set => SetAndRaise(ref _t4,value);
        }

        int _t4 ;
        public string Query
        {
            get => _query; set => SetAndRaise(ref _query,value);
        }

        string _query = "";
        public string Parametres
        {
            get => _parametres; set => SetAndRaise(ref _parametres,value);
        }

        string _parametres = "";
        public string TaillesColonnes
        {
            get => _taillesColonnes; set => SetAndRaise(ref _taillesColonnes,value);
        }

        string _taillesColonnes = "";
        //public string Droit
        //{
        //    get => _droit; set => SetAndRaise(ref _droit,value);
        //}
        //private string _droit = "";
        //public string Cache
        //{
        //    get => _cache; set => SetAndRaise(ref _cache,value);
        //}
        //private string _cache = "";

        [Ignore]
        public string Caption => _caption.Value;
        ObservableAsPropertyHelper<string> _caption;


        [Ignore]
        public string IconPath => "";

    }
}