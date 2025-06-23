using HLab.Core.Annotations;
using HLab.Erp.Base.Data;
using HLab.Erp.Core;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Products;

public class UnitsListViewModel(EntityListViewModel<Unit>.Injector i) : EntityListViewModel<Unit>(i,
   c => c
      .Column("Name")
      .Header("{Name}").IconPath("Icons/Entities/Unit")
      .Width(250)
      .Localize(e => e.Name)
      .Icon(p => p.IconPath)
      .Link(e => e.Name)
      .Filter()), IMvvmContextProvider
{
   public class Bootloader : ParamBootloader
   {
      //public override void Load(IBootContext bootstrapper)
      //{
      //    Menu.RegisterMenu("param/units", "{Units}", null, "Icons/Entities/Unit");
      //    base.Load(bootstrapper);
      //}
      public override string MenuPath => "param/units";

      protected override BootState Load()
      {
         Menu.RegisterMenu(MenuPath, "{Units}", null, IconPath);
         return base.Load();
      }
   }

   //Todo : rights to configure units
   protected override bool AddCanExecute(Action<string> errorAction) => true;
   protected override bool DeleteCanExecute(Unit inn, Action<string> errorAction) => true;

   public void ConfigureMvvmContext(IMvvmContext ctx)
   {
   }

}