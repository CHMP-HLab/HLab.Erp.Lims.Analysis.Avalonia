using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using DynamicData;
using DynamicData.Binding;
using HLab.Base.ReactiveUI;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Core.ListFilters;
using HLab.Erp.Workflows;
using HLab.Erp.Workflows.Interfaces;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Filters;

public class ConformityFilter : Filter<ConformityState>, IWorkflowFilter
{
    static readonly MethodInfo ContainsMethod = typeof(List<ConformityState>).GetMethod("Contains", new[] { typeof(ConformityState) });

    public class ConformityEntry : ReactiveModel
    {
        public bool Selected
        {
            get => _selected;
            set => SetAndRaise(ref _selected, value);
        }
        bool _selected;

        public ConformityState State { get; set; }

        public string IconPath => State.IconPath();
        public string Caption => State.Caption();
    }

    public ReadOnlyObservableCollection<ConformityEntry> List { get; }
    readonly ObservableCollection<ConformityEntry> _list = new();

    public ConformityFilter()
    {
        List = new(_list);

        _list.ToObservableChangeSet().AutoRefresh(e => e.Selected).Subscribe(e =>
        {
            Update?.Invoke();
        });

        this.WhenAnyValue(e => e.Enabled).Subscribe(e =>
        {
            Update?.Invoke();
        });

        var values = Enum.GetValues(typeof(ConformityState)).Cast<ConformityState>();

        foreach (var state in values)
        {
            _list.Add(new ConformityEntry { State = state });
        }
    }

    public ConformityState Selected { get; set; }


    public override Expression<Func<T, bool>> Match<T>(Expression<Func<T, ConformityState>> getter)
    {
        if (!Enabled) return null;

        var entity = getter.Parameters[0];
        var value = Expression.Constant(List.Where(e => e.Selected).Select(e => e.State).ToList(), typeof(List<ConformityState>));

        var ex = Expression.Call(value, ContainsMethod, getter.Body);

        return Expression.Lambda<Func<T, bool>>(ex, entity);
    }

    public override Func<T, bool> PostMatch<T>(Func<T, ConformityState> getter)
    {
        if (!Enabled) return t => true;

        var values = List.Where(e => e.Selected).Select(e => e.State).ToList();

        return t => values.Contains(getter(t));
    }
    public override XElement ToXml()
    {
        var element = base.ToXml();

        foreach(var value in List)
        {
            if(value.Selected)
            {
                var xState = new XElement(value.State.ToString());
                element.Add(xState);
            }
        }
        return element;
    }
    public override void FromXml(XElement element)
    {
        foreach(var state in _list)
        {
            state.Selected = element.Elements().Any(e => e.Name == state.State.ToString());
        }
    }    
}