using Grace.DependencyInjection;
using HLab.Base.Wpf.Themes;
using HLab.Bugs.Wpf;
using HLab.Core;
using HLab.Core.Annotations;
using HLab.Core.DebugTools;
using HLab.Erp.Acl;
using HLab.Erp.Acl.AuditTrails;
using HLab.Erp.Acl.LoginServices;
using HLab.Erp.Acl.Windows;
using HLab.Erp.Base.Data;
using HLab.Erp.Core;
using HLab.Erp.Core.DragDrops;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.WebService;
using HLab.Erp.Core.Wpf.DragDrops;
using HLab.Erp.Core.Wpf.EntityLists;
using HLab.Erp.Core.Wpf.Localization;
using HLab.Erp.Core.Wpf.WebService;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Icons.Wpf.Icons;
using HLab.Mvvm;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application;
using HLab.Mvvm.Application.Documents;
using HLab.Mvvm.Application.Menus;
using HLab.Mvvm.Application.Messages;
using HLab.Mvvm.Application.Wpf;
using HLab.Mvvm.Wpf;
using HLab.Options;
using HLab.Ui.Wpf;
using HLab.UI;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Concurrency;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;
using MessageBus = HLab.Core.MessageBus;

namespace HLab.Erp.Lims.Analysis.Wpf;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    protected override async void OnStartup(StartupEventArgs e)
    {
        try
        {
            base.OnStartup(e);

            var theme = new ThemeService(Resources);

            var container = new DependencyInjectionContainer();

            ListFilterConfiguratorWpfImplementation.Initialize();
            UiWpfImplementation.Initialize();

            // Needed for
            RxSchedulers.MainThreadScheduler = RxSchedulers.MainThreadScheduler = new SynchronizationContextScheduler(
                SynchronizationContext.Current ??
                new DispatcherSynchronizationContext()
            );


            container.Configure(c =>
            {
             c.Export<OptionsServices>().As<IOptionsService>().Lifestyle.Singleton();
             //                    c.Export<EventHandlerServiceWpf>().As<IEventHandlerService>().Lifestyle.Singleton();
             c.Export<MessageBus>().As<IMessagesService>().Lifestyle.Singleton();

             c.Export<AclService>().As<IAclService>().Lifestyle.Singleton();
             c.Export<CryptService>().As<ICryptService>().Lifestyle.Singleton();
             c.Export<AclHelperWindows>().As<IAclHelper>().Lifestyle.Singleton();
             c.Export<DebugLogger>().As<IDebugLogger>().Lifestyle.Singleton();
             c.Export<DataService>().As<IDataService>().Lifestyle.Singleton();

             // WPF
             c.Export<DragDropServiceWpf>().As<IDragDropService>().Lifestyle.Singleton();
             c.Export<WpfDocumentService>().As<IDocumentService>().Lifestyle.Singleton();
             c.Export<MvvmWpfImpl>().As<IMvvmPlatformImpl>().Lifestyle.Singleton();


             c.Export<DialogService>().As<IDialogService>().Lifestyle.Singleton();
             /*
                                 c.Export<CurrencyService>().As<ICurrencyService>().Lifestyle.Singleton();
                                 c.Export<GraphService>().As<IGraphService>().Lifestyle.Singleton();
             */
             c.Export<UnitService>().As<IUnitService>().Lifestyle.Singleton();

             c.Export<BrowserViewModel>().As<IBrowserService>().Lifestyle.Singleton();
             c.Export<IconService>().As<IIconService>().Lifestyle.Singleton();
             c.Export<LocalizationService>().As<ILocalizationService>().Lifestyle.Singleton();
             c.Export<MvvmService>().As<IMvvmService>().Lifestyle.Singleton();
             c.Export<ApplicationInfoService>().As<IApplicationInfoService>().Lifestyle.Singleton();
             c.Export<DocumentPresenter>().As<IDocumentPresenter>();
             c.Export<MenuService>().As<IMenuService>().Lifestyle.Singleton();
             c.Export<LocalizeFromDb>().As<LocalizeFromDb>().Lifestyle.Singleton();

             c.Export<MainWpfViewModel>().As<MainWpfViewModel>().Lifestyle.Singleton();

             c.Export<LoginViewModel>().As<ILoginViewModel>();
             c.Export<AuditTrailMotivationViewModel>().As<IAuditTrailProvider>();
             c.Export<SelectedMessage>().As<ISelectedMessage>();
             c.Export<GuiTimer>().As<IGuiTimer>();

             c.Export(typeof(EntityListHelper<>)).As(typeof(IEntityListHelper<>));
             c.Export(typeof(ColumnsProvider<>)).As(typeof(IColumnsProvider<>));
             c.Export(typeof(ObservableQuery<>)).As(typeof(IObservableQuery<>));
             c.Export(typeof(DataLocker<>)).As(typeof(IDataLocker<>));

             var parser = new AssemblyParser();

             parser.LoadReferencedAssemblies("HLab");


             ////var a0 = boot.LoadDll("HLab.Erp.Core.Wpf");
             var a01 = parser.LoadDll("HLab.Options.Wpf");
             var a3 = parser.LoadDll("HLab.Notify.Wpf");
             var a2 = parser.LoadDll("HLab.Erp.Base.Wpf");
             ////  var b0 = boot.LoadDll("HLab.Mvvm");
             var c0 = parser.LoadDll("HLab.Erp.Base");
             var d1 = parser.LoadDll("HLab.Erp.Data.Wpf");
             var d0 = parser.LoadDll("HLab.Erp.Base.Data");
             var e0 = parser.LoadDll("HLab.Erp.Acl.Wpf");
             var a1 = parser.LoadDll("HLab.Erp.Workflows.Wpf");
             var g0 = parser.LoadDll("HLab.Erp.Lims.Analysis.Data");
             var g2 = parser.LoadDll("HLab.Erp.Lims.Analysis.Module");
             parser.LoadDll("HLab.Erp.Lims.Analysis");
             ////var g1 = boot.LoadDll("HLab.Erp.Lims.Monographs.Module");

             parser.LoadModules();

             parser.Add<IView>(t => c.Export(t).As(typeof(IView)));
             parser.Add<IViewModel>(t => c.Export(t).As(typeof(IViewModel)));
             parser.Add<Bootloader>(t => c.Export(t).As(typeof(Bootloader)));

             IEntityListViewModel<Manufacturer> a = null;

             //                    parser.Add<IToolGraphBlock>(t => c.Export(t).As(typeof(IToolGraphBlock)));
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
             //                    c.ExportInitialize<GraphElement>(o => o.MvvmService = container.Locate<IMvvmService>());
             c.ExportInitialize<NestedBootloader>(o =>
                {
                    o.Menu = container.Locate<IMenuService>();
                    o.Docs = container.Locate<IDocumentService>();
                });
         });


            // TODO Urgent
            //options.AddProvider(new OptionsProviderRegistry());

            //STATIC IMPORTS//
            // NotifyHelper.EventHandlerService = container.Locate<IEventHandlerService>();


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

            var acl = container.Locate<IAclHelper>();


            var boot = new Bootstrapper(container.Locate<IEnumerable<Bootloader>>);
            await boot.BootAsync();
        }
        catch (Exception ex)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                var view = new ExceptionView
                {


                    Exception = ex
                    // TODO store token in db
                    ,
                    Token = ""
                };
                view.ShowDialog();
#if DEBUG
                //throw;
                ExceptionDispatchInfo.Capture(ex).Throw();
#endif

            });
        }
    }


}