using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Grace.DependencyInjection;
using HLab.Base.Avalonia.Themes;
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
using HLab.Erp.Acl.AuditTrails;
using HLab.Erp.Acl.Windows;
using HLab.Icons.Avalonia;
using HLab.Mvvm.Avalonia;
using HLab.Erp.Acl.Avalonia.LoginServices;
using HLab.Erp.Acl.LoginServices;

namespace HLab.Erp.Lims.Analysis.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        try{
                var theme = new ThemeService(Resources);

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

                    /*
                    c.Export<MainAvaloniaViewModel>().As<MainAvaloniaViewModel>().Lifestyle.Singleton();


                    c.Export<LocalizeFromDb>().As<LocalizeFromDb>().Lifestyle.Singleton();
                    c.Export<CurrencyService>().As<ICurrencyService>().Lifestyle.Singleton();
                    c.Export<EventHandlerServiceWpf>().As<IEventHandlerService>().Lifestyle.Singleton();
                    c.Export<DragDropServiceWpf>().As<IDragDropService>().Lifestyle.Singleton();
                    c.Export<GraphService>().As<IGraphService>().Lifestyle.Singleton();

                    c.Export<BrowserViewModel>().As<IBrowserService>().Lifestyle.Singleton();

                    c.Export<MvvmServiceWpf>().As<IMvvmService>().Lifestyle.Singleton();
                    c.Export<DocumentServiceWpf>().As<IDocumentService>().Lifestyle.Singleton();
                    c.Export<DocumentPresenter>().As<IDocumentPresenter>();
                    c.Export<MenuService>().As<IMenuService>().Lifestyle.Singleton();


                    c.Export<SelectedMessage>().As<ISelectedMessage>();

                    c.Export(typeof(EntityListHelper<>)).As(typeof(IEntityListHelper<>));
                    c.Export(typeof(ColumnsProvider<>)).As(typeof(IColumnsProvider<>));
                    */

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

                _ = SampleWorkflow.Production;
                _ = SampleTestWorkflow.ValidatedResults;
                _ = SampleTestResultWorkflow.Checked;

                var options = container.Locate<IOptionsService>();
                options.OptionsPath = "HLab.Erp";
                //options.AddProvider(new OptionsProviderRegistry());

                //STATIC IMPORTS//
                // NotifyHelper.EventHandlerService = container.Locate<IEventHandlerService>();
                WorkflowAnalysisExtension.Acl = container.Locate<IAclService>();

                var mvvm = container.Locate<IMvvmService>();

            /*
                mvvm.Register(typeof(Customer), typeof(CustomerViewModel), typeof(IDocumentViewClass), typeof(DefaultViewMode));
                mvvm.Register(typeof(Manufacturer), typeof(ManufacturerViewModel), typeof(IDocumentViewClass), typeof(DefaultViewMode));
            */
                mvvm.RegisterAsync();

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
                boot.BootAsync();
            }
            catch (Exception ex)
            {
            /* TODO
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
            */
        }
    }
}
    

