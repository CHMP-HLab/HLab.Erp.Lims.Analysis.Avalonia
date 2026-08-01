using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Grace.DependencyInjection;
using HLab.Core;
using HLab.Core.Annotations;
using HLab.Core.DebugTools;
using HLab.Erp.Acl;
using HLab.Erp.Base.Data;
using HLab.Erp.Core;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Mvvm;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application;
using HLab.Mvvm.Application.Documents;
using HLab.Mvvm.Application.Menus;
using HLab.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using HLab.Erp.Acl.AuditTrails;
using HLab.Erp.Acl.Windows;
using HLab.Icons.Avalonia;
using HLab.Mvvm.Avalonia;
using HLab.Erp.Acl.Avalonia.LoginServices;
using HLab.Erp.Acl.LoginServices;
using HLab.Mvvm.Application.Avalonia;
using HLab.Mvvm.Application.Messages;
using HLab.Theme.Avalonia;
using HLab.Ui.Avalonia;
using ReactiveUI;
using ReactiveUI.Avalonia;
using MessageBus = HLab.Core.MessageBus;

namespace HLab.Erp.Lims.Analysis.Avalonia;

public partial class App : Application
{
   public override void Initialize()
   {
      AvaloniaXamlLoader.Load(this);
   }

   public override async void OnFrameworkInitializationCompleted()
   {
      try
      {
         var theme = new ThemeService(Resources);

         UiAvaloniaImplementation.Initialize();
         RxSchedulers.MainThreadScheduler = AvaloniaScheduler.Instance;

         var container = new DependencyInjectionContainer();

         container.Configure(c =>
         {
            c.Export<OptionsServices>().As<IOptionsService>().Lifestyle.Singleton();
            c.Export<MessageBus>().As<IMessagesService>().Lifestyle.Singleton();
            c.Export<AclService>().As<IAclService>().Lifestyle.Singleton();
            c.Export<DataService>().As<IDataService>().Lifestyle.Singleton();

            c.Export<ApplicationInfoService>().As<IApplicationInfoService>().Lifestyle.Singleton();
            c.Export<UnitService>().As<IUnitService>().Lifestyle.Singleton();
            c.Export<DebugLogger>().As<IDebugLogger>().Lifestyle.Singleton();
            c.Export<LocalizationService>().As<ILocalizationService>().Lifestyle.Singleton();
            c.Export(typeof(ObservableQuery<>)).As(typeof(IObservableQuery<>));
            c.Export(typeof(DataLocker<>)).As(typeof(IDataLocker<>));

            c.Export<AclHelperWindows>().As<IAclHelper>().Lifestyle.Singleton();

            c.Export<DialogService>().As<IDialogService>().Lifestyle.Singleton();
            c.Export<IconService>().As<IIconService>().Lifestyle.Singleton();
            c.Export<LoginViewModel>().As<ILoginViewModel>();
            c.Export<AuditTrailMotivationViewModel>().As<IAuditTrailProvider>();

            c.Export<MvvmService>().As<IMvvmService>().Lifestyle.Singleton();

            c.Export<CryptService>().As<ICryptService>().Lifestyle.Singleton();
            c.Export<MvvmAvaloniaImpl>().As<MvvmAvaloniaImpl>().As<IMvvmPlatformImpl>().Lifestyle.Singleton();

            c.Export<AvaloniaDocumentService>().As<IDocumentService>().Lifestyle.Singleton();
            c.Export<DocumentPresenterViewModel>().As<IDocumentPresenter>();
            c.Export<AvaloniaMenuService>().As<IMenuService>().Lifestyle.Singleton();
            c.Export<AvaloniaApplicationViewModel>().As<IApplicationViewModel>().Lifestyle.Singleton();
            c.Export<SelectedMessage>().As<ISelectedMessage>();

            /* TODO phase 3 : portage HLab.Erp.Core.Avalonia
            c.Export<LocalizeFromDb>().As<LocalizeFromDb>().Lifestyle.Singleton();
            c.Export<CurrencyService>().As<ICurrencyService>().Lifestyle.Singleton();
            c.Export<DragDropServiceAvalonia>().As<IDragDropService>().Lifestyle.Singleton();
            c.Export<BrowserViewModel>().As<IBrowserService>().Lifestyle.Singleton();

            c.Export(typeof(EntityListHelper<>)).As(typeof(IEntityListHelper<>));
            c.Export(typeof(ColumnsProvider<>)).As(typeof(IColumnsProvider<>));
            */

            var parser = new AssemblyParser();

            parser.LoadReferencedAssemblies("HLab");

            // Assemblies jamais référencés par le code : le compilateur les élague
            // des métadonnées, il faut les charger explicitement.
            parser.LoadDll("HLab.Options.Wpf"); // provider d'options (registre), pas de dépendance WPF
            parser.LoadDll("HLab.Erp.Acl.Avalonia"); // LoginView, audit trail
            parser.LoadDll("HLab.Erp.Workflow.Avalonia"); // vues workflow

            parser.LoadModules();

            parser.Add<IView>(t => c.Export(t).As(typeof(IView)));
            parser.Add<IViewModel>(t => c.Export(t).As(typeof(IViewModel)));
            parser.Add<Bootloader>(t => c.Export(t).As(typeof(Bootloader)));

            //parser.Add<IToolGraphBlock>(t => c.Export(t).As(typeof(IToolGraphBlock)));
            parser.Add<IEntityListViewModel>(listType =>
                   {
                      if (listType.IsGenericType) return;

                      foreach (var interfaceType in listType.GetInterfaces().Where(i => i.IsGenericType))
                      {
                         if (interfaceType.GetGenericTypeDefinition() != typeof(IEntityListViewModel<>)) continue;
                         var entityType = interfaceType.GetGenericArguments()[0];

                         c.Export(listType).As(interfaceType);
                      }
                   });

            parser.Parse();

            c.ExportInitialize<Entity>(o => o.DataService = container.Locate<IDataService>());
            //c.ExportInitialize<GraphElement>(o => o.MvvmService = container.Locate<IMvvmService>());
            c.ExportInitialize<NestedBootloader>(o =>
                   {
                      o.Menu = container.Locate<IMenuService>();
                      o.Docs = container.Locate<IDocumentService>();
                   });
         });

         //options.AddProvider(new OptionsProviderRegistry());

         //STATIC IMPORTS//
         // NotifyHelper.EventHandlerService = container.Locate<IEventHandlerService>();

         /*
             mvvm.Register(typeof(Customer), typeof(CustomerViewModel), typeof(IDocumentViewClass), typeof(DefaultViewMode));
             mvvm.Register(typeof(Manufacturer), typeof(ManufacturerViewModel), typeof(IDocumentViewClass), typeof(DefaultViewMode));
         */

         //var doc = container.Locate<IDocumentService>();
         //doc.MainViewModel = container.Locate<MainWpfViewModel>();

         var info = container.Locate<IApplicationInfoService>();
         info.PropertyChanged += (s, a) =>
         {
            if (a.PropertyName == "Theme")
            {
               theme.SetTheme(info.Theme);
            }
         };
         theme.SetTheme(info.Theme);


         var boot = new Bootstrapper(container.Locate<IEnumerable<HLab.Core.Annotations.Bootloader>>);

         // Boot once the Avalonia main loop is running : contrary to WPF where OnStartup
         // runs inside Application.Run, this method completes *before* the lifetime starts,
         // and awaiting the boot here would spin the UI thread before any window can show.
         global::Avalonia.Threading.Dispatcher.UIThread.Post(async () =>
         {
            try
            {
               await boot.BootAsync();
            }
            catch (Exception ex)
            {
               ShowBootError(ex);
            }
         });
      }
      catch (Exception ex)
      {
         ShowBootError(ex);
      }
   }

   void ShowBootError(Exception ex)
   {
      Console.Error.WriteLine(ex);

      if (ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;

      var window = new Window
      {
         Title = "LIMS - Erreur de démarrage",
         Width = 900,
         Height = 600,
         Content = new TextBox
         {
            Text = ex.ToString(),
            IsReadOnly = true,
            AcceptsReturn = true,
            TextWrapping = TextWrapping.Wrap
         }
      };

      desktop.MainWindow = window;
      window.Show();
   }
}


