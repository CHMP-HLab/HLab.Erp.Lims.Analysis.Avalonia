﻿using HLab.Base;
using HLab.Erp.Acl;
using HLab.Erp.Core;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.TestClasses;

public class TestClassesListViewModel : Core.EntityLists.EntityListViewModel<TestClass>, IMvvmContextProvider
{
    public class Bootloader : NestedBootloader
    {
        public override string MenuPath => "param";
    }

    readonly IAclService _acl;

    protected override bool AddCanExecute(Action<string> errorAction) =>
        _acl.IsGranted(AnalysisRights.AnalysisTestClassCreate, errorAction);

    protected override bool DeleteCanExecute(TestClass arg, Action<string> errorAction) =>
        arg!=null && _acl.IsGranted(AnalysisRights.AnalysisTestClassCreate, errorAction);

    protected override bool ExportCanExecute(Action<string> errorAction) =>
        _acl.IsGranted(AnalysisRights.AnalysisTestClassCreate, errorAction);

    protected override bool ImportCanExecute(Action<string> errorAction) =>
        _acl.IsGranted(AnalysisRights.AnalysisTestClassCreate, errorAction);


    static async Task ImportAsync(IDataService data, TestClass import, TestClass current)
    {
        current.IconPath = import.IconPath;
        current.Version = import.Version;
        current.Code = import.Code;

    }

    protected override async Task ImportAsync(IDataService data, TestClass import)
    {
        var current = await data.FetchOneAsync<TestClass>(i => i.Name == import.Name);
        if (current != null)
        {
            if(Version.TryParse(current.Version, out var currentVersion))
            {
                if(Version.TryParse(import.Version, out var importVersion))
                {
                    if (importVersion > currentVersion)
                    {
                        import.CopyPrimitivesTo(current);
                        await data.UpdateAsync(current,"IconPath","Version","Code");
                    }
                }
            }
        }
        else
        {
            _ = await data.AddAsync<TestClass>(import.CopyPrimitivesTo);

        }
    }

    public TestClassesListViewModel(IAclService acl, Injector i) : base(i, c => c
        .Column("Name")
        .Header("{Name}").Width(200)
        .Content(s => s.Name)
        .Icon(s => s.IconPath)
        .Link(c => c.Name)
        .Filter()

        .Column().Link(c => c.Version).Filter()

        .Column("Category")
        .Header("{Category}")
        .Width(150).Content(s => s.Category.Name)
        .OrderBy(s => s.Category?.Name)           
    )
    {
        _acl = acl;
    }

    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }
}