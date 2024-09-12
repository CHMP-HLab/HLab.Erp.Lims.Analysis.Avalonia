using System.Threading.Tasks;
using System.Windows.Input;
using HLab.Base.ReactiveUI;
using HLab.Core.Annotations;
using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Products.Tools;
using HLab.Mvvm.Application.Documents;
using HLab.Mvvm.Application.Menus;

namespace HLab.Erp.Lims.Analysis.Wpf.Products.Tools;

public class ProductToolsModule : ReactiveModel, IBootloader
{
    readonly IDocumentService _docs;
    readonly IAclService _acl;
    readonly IMenuService _menu;

    public ProductToolsModule(IDocumentService docs, IAclService acl, IMenuService menu)
    {
        _docs = docs;
        _acl = acl;
        _menu = menu;
        
        OpenCommand = ReactiveUI.ReactiveCommand
            .CreateFromTask(e => _docs.OpenDocumentAsync(typeof(ProductToolsViewModel)));
    }

    public ICommand OpenCommand { get; }

    protected virtual string IconPath => "Icons/Entities/";

    public async virtual Task LoadAsync(IBootContext b)
    {
        if (b.WaitDependency("BootLoaderErpWpf")) return;

        if (_acl.Connection == null)
        {
            if(!_acl.Cancelled) b.Requeue();
            return;
        }

        if(!_acl.IsGranted(AclRights.ManageUser)) return;

        _menu.RegisterMenu("tools/ProductTools", "{Product Tools}",
            OpenCommand,
            "icons/tools/ProductTools");
    }
}