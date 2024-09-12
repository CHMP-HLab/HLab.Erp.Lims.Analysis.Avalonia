using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Xml;
using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Data.Entities;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Products;

public class InnViewModel : ListableEntityViewModel<Inn>
{

    //public string SubTitle => _subTitle.Get();
    //private string _subTitle = H.Property<string>(c => c
    //    .Set(e => e.GetSubTitle )
    //    .On(e => e.Model.Name)
    //    .Update()
    //);
    //private string GetSubTitle => $"{Model?.Variant}\n{Model?.Form?.Name}";


    public override string IconPath => _iconPath.Value;
    private readonly ObservableAsPropertyHelper<string> _iconPath;


    string GetIconPath => Model?.IconPath ?? Model?.IconPath ?? base.IconPath;


    readonly Func<Inn, InnProductComponentListViewModel> _getComponents;

    public InnViewModel(Injector i, Func<Inn, InnProductComponentListViewModel> getComponents) : base(i)
    {
        _getComponents = getComponents;

        _iconPath = this.WhenAnyValue(e => e.Model.IconPath)
            .Select(iconPath => iconPath ?? Model?.IconPath ?? base.IconPath)
            .ToProperty(this, e => e.IconPath);

        this.WhenAnyValue(e => e.Locker.IsActive)
            .Do(lockerIsActive =>
            {
                if (Components != null)
                    Components.EditMode = lockerIsActive;

            }).Subscribe();

        _components = this.WhenAnyValue(e => e.Model)
            .Select(inn =>
            {
                var components = _getComponents?.Invoke(inn);
                //if (tests != null) tests.List.CollectionChanged += e.List_CollectionChanged;
                return components;
            })
            .ToProperty(this, e => e.Components);

        WikiCommand = ReactiveCommand.Create(Wiki);
    }


    public InnProductComponentListViewModel Components => _components.Value;
    readonly ObservableAsPropertyHelper<InnProductComponentListViewModel> _components;

    public ICommand WikiCommand { get; }

    void Wiki()
    {
        var url = $"https://en.wikipedia.org/w/api.php?action=query&prop=revisions&titles={Model.Name}&rvslots=*&rvprop=content&formatversion=2&format=xml";
        XmlDocument doc = new XmlDocument();

        try
        {
            doc.Load(url);
        }
        catch { return; }

        XmlNode node = doc;
        // if (node is not { HasChildNodes: true }) return;

        if (!GetNode(ref node, "api", "query", "pages", "page", "revisions", "rev", "slots", "slot")) return;

        var wiki = node.InnerText;

        Model.CasNumber = GetWikiValue(wiki, "CAS_number");
    }

    public static string GetWikiValue(string wiki, string name)
    {
        var regex = new Regex(@$"\| *{name} *=(.*?)\n");
        var result = regex.Match(wiki);
        if (result.Groups.Count <= 1) return null;

        return result.Groups[1].Value.Trim();
    }

    public static bool GetNode(ref XmlNode node, params string[] names)
    {
        foreach (var name in names)
        {
            if (!GetNode(ref node, name)) return false;
        }
        return true;
    }

    public static bool GetNode(ref XmlNode node, string name)
    {
        if (node is not { HasChildNodes: true }) return false;
        foreach (var child in node.ChildNodes.OfType<XmlNode>())
        {
            if (child.Name != name) continue;
            node = child;
            return true;
        }
        return false;
    }

    //public ProductWorkflow Workflow => _workflow.Get();
    //private ProductWorkflow _workflow = H.Property<ProductWorkflow>(c => c
    //    .On(e => e.Model)
    //    .OnNotNull(e => e.Locker)
    //    .Set(vm => new ProductWorkflow(vm.Model,vm.Locker))
    //);
}