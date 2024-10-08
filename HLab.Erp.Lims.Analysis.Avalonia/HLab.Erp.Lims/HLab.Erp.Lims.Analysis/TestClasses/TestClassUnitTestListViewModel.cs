﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Wpf.TestClasses;

public class TestClassUnitTestListViewModel : Core.EntityLists.EntityListViewModel<TestClassUnitTest>, IMvvmContextProvider
{
    readonly ObservableCollection<int> _failedTests = [];
    readonly Dictionary<int, string> _errors = new();

    readonly ObservableCollection<int> _passedTests = [];



    public void AddError(int idx, string error)
    {
        if (_errors.ContainsKey(idx))
        {
            _errors.Remove(idx);
            _failedTests.Remove(idx);
        }
        if (_passedTests.Contains(idx)) _passedTests.Remove(idx);
        _errors.Add(idx,error);
        _failedTests.Add(idx);
    }

    public void AddPassed(int idx)
    {
        if (_errors.ContainsKey(idx))
        {
            _errors.Remove(idx);
            _failedTests.Remove(idx);
        }
        if (_passedTests.Contains(idx)) return;
        _passedTests.Add(idx);
    }

    public TestClassUnitTestListViewModel(Injector i, TestClass testClass) : base(i, c =>
        {
            return c.List<TestClassUnitTestListViewModel>(out var list)
                    .StaticFilter(u =>u.TestClassId == testClass.Id && list.Id>=0)
                    .Column("Name")
                    .Header("{Name}").Width(200)
                    .Link(s => s.Name)
                    .Filter()
                    .Column("error")
                    .Header("{Error}").Content(s => list._failedTests.Contains(s.Id) ? list._errors[s.Id] : "OK").Width(150)
                    .Icon(s => list._failedTests.Contains(s.Id) ? "Icons/Conformity/CheckFailed" : list._passedTests.Contains(s.Id) ? "Icons/Conformity/CheckPassed" : "Icons/Conformity/Invalid")
                ;
        }
    )
    {
        _failedTests.CollectionChanged += FailedTests_CollectionChanged;
        _passedTests.CollectionChanged += FailedTests_CollectionChanged;
    }
    protected override bool AddCanExecute(Action<string> errorAction) => true;

    protected override bool DeleteCanExecute(TestClassUnitTest unitTest,Action<string> errorAction) => Selected !=null || (SelectedIds?.Any()??false);

    void FailedTests_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
    }

    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }
}