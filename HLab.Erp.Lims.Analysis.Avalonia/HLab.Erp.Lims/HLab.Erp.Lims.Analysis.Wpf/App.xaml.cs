using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Windows;
using System.Windows.Threading;
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
using HLab.Erp.Base.Wpf.Entities.Customers;
using HLab.Erp.Core;
using HLab.Erp.Core.DragDrops;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Core.WebService;
using HLab.Erp.Core.Wpf.DragDrops;
using HLab.Erp.Core.Wpf.EntityLists;
using HLab.Erp.Core.Wpf.Localization;
using HLab.Erp.Core.Wpf.WebService;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Erp.Lims.Analysis.Manufacturers;
using HLab.Erp.Lims.Analysis.Module.TestClasses;
using HLab.Erp.Lims.Analysis.Wpf.TestClasses;
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

            ListFilterConfiguratorExtension.Platform = new ListFilterConfiguratorWpfImplementation();

            container.Configure(c =>
            {
                c.Export<OptionsServices>().As<IOptionsService>().Lifestyle.Singleton();
//                    c.Export<EventHandlerServiceWpf>().As<IEventHandlerService>().Lifestyle.Singleton();
                c.Export<MessageBus>().As<IMessagesService>().Lifestyle.Singleton();

                c.Export<AclService>().As<IAclService>().Lifestyle.Singleton();
                c.Export<AclHelperWindows>().As<IAclHelper>().Lifestyle.Singleton();
                c.Export<DebugLogger>().As<IDebugLogger>().Lifestyle.Singleton();
                c.Export<DataService>().As<IDataService>().Lifestyle.Singleton();

                // WPF
                c.Export<DragDropServiceWpf>().As<IDragDropService>().Lifestyle.Singleton();
                c.Export<WpfDocumentService>().As<IDocumentService>().Lifestyle.Singleton();


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
                c.Export<MvvmWpfImpl>().As<IMvvmPlatformImpl>().Lifestyle.Singleton();
                c.Export<ApplicationInfoService>().As<IApplicationInfoService>().Lifestyle.Singleton();
                c.Export<DocumentPresenter>().As<IDocumentPresenter>();
                c.Export<MenuService>().As<IMenuService>().Lifestyle.Singleton();
                c.Export<LocalizeFromDb>().As<LocalizeFromDb>().Lifestyle.Singleton();

                c.Export<MainWpfViewModel>().As<MainWpfViewModel>().Lifestyle.Singleton();

                c.Export<LoginViewModel>().As<ILoginViewModel>();
                c.Export<AuditTrailMotivationViewModel>().As<IAuditTrailProvider>();
                c.Export<SelectedMessage>().As<ISelectedMessage>();

                c.Export(typeof(EntityListHelper<>)).As(typeof(IEntityListHelper<>));
                c.Export(typeof(ColumnsProvider<>)).As(typeof(IColumnsProvider<>));
                c.Export(typeof(ObservableQuery<>)).As(typeof(IObservableQuery<>));
                c.Export(typeof(DataLocker<>)).As(typeof(IDataLocker<>));

                var parser = new AssemblyParser();

                //var a0 = boot.LoadDll("HLab.Erp.Core.Wpf");
                var a01 = parser.LoadDll("HLab.Options.Wpf");
                var a3 = parser.LoadDll("HLab.Notify.Wpf");
                var a2 = parser.LoadDll("HLab.Erp.Base.Wpf");
                //  var b0 = boot.LoadDll("HLab.Mvvm");
                //  var c0 = boot.LoadDll("HLab.Mvvm.Wpf");
                //  var d0 = boot.LoadDll("HLab.Erp.Data");
                var d1 = parser.LoadDll("HLab.Erp.Data.Wpf");
                var e0 = parser.LoadDll("HLab.Erp.Acl.Wpf");
                var a1 = parser.LoadDll("HLab.Erp.Workflows.Wpf");
                var g0 = parser.LoadDll("HLab.Erp.Lims.Analysis.Data");
                var g2 = parser.LoadDll("HLab.Erp.Lims.Analysis.Module");
                //var g1 = boot.LoadDll("HLab.Erp.Lims.Monographs.Module");

                parser.LoadModules();

                parser.Add<IView>(t => c.Export(t).As(typeof(IView)));
                parser.Add<IViewModel>(t => c.Export(t).As(typeof(IViewModel)));
                parser.Add<IBootloader>(t => c.Export(t).As(typeof(IBootloader)));

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

            _ = SampleWorkflow.Production;
            _ = SampleTestWorkflow.ValidatedResults;
            _ = SampleTestResultWorkflow.Checked;

            var options = container.Locate<IOptionsService>();
            options.OptionsPath = "HLab.Erp";
            // TODO Urgent
            //options.AddProvider(new OptionsProviderRegistry());

            //STATIC IMPORTS//
            // NotifyHelper.EventHandlerService = container.Locate<IEventHandlerService>();
            WorkflowAnalysisExtension.Acl = container.Locate<IAclService>();

            var mvvm = container.Locate<IMvvmService>();

            await mvvm.RegisterAsync(typeof(Customer), typeof(CustomerViewModel), typeof(IDocumentViewClass), typeof(DefaultViewMode));
            await mvvm.RegisterAsync(typeof(Manufacturer), typeof(ManufacturerViewModel), typeof(IDocumentViewClass), typeof(DefaultViewMode));
            await mvvm.RegisterAsync();

            //var doc = container.Locate<IDocumentService>();
            //doc.MainViewModel = container.Locate<MainWpfViewModel>();

            var info = container.Locate<IApplicationInfoService>();
            info.PropertyChanged += (s,a) =>
            {
                if (a.PropertyName == "Theme")
                {
                    theme.SetTheme(info.Theme);
                }
            };
            theme.SetTheme(info.Theme);


            var boot = new Bootstrapper(container.Locate<IEnumerable<IBootloader>>);
            await boot.BootAsync();
        }
        catch (Exception ex)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                var view = new ExceptionView {


                    Exception = ex 
                    // TODO store token in db
                    , Token = ""
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