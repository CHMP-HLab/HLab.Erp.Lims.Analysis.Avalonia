using System.Windows.Input;
using HLab.Core.Annotations;
using HLab.Erp.Acl;
using HLab.Mvvm.Application.Documents;
using HLab.Mvvm.Application.Menus;

namespace HLab.Erp.Lims.Analysis.Products.Tools;

public class ProductToolsModule : Bootloader
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

   protected override BootState Load()
   {
      if (WaitingForBootloader("BootLoaderErpWpf")) return BootState.Requeue;

      if (_acl.Connection == null)
      {
         return _acl.Cancelled ? BootState.Cancel : BootState.Requeue;
      }

      if (_acl.IsGranted(AclRights.ManageUser))
      {
         _menu.RegisterMenu("tools/ProductTools", "{Product Tools}",
             OpenCommand,
             "icons/tools/ProductTools");
      }

      return base.Load();
   }
}