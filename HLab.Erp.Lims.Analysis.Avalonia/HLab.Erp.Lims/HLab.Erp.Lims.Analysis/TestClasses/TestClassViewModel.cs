using System.Reactive.Linq;
using System.Windows.Input;
using HLab.Base.ReactiveUI;
using HLab.Erp.Acl;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Lims.Analysis.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.FormClasses;
using HLab.Erp.Lims.Analysis.Wpf.TestClasses;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.TestClasses;

public class TestClassViewModel : ListableEntityViewModel<TestClass>, IFormHelperProvider
{
    public override string Header => _header.Value;
    readonly ObservableAsPropertyHelper<string> _header;

    public TestClassViewModel(Injector i, Func<TestClass, TestClassUnitTestListViewModel> getUnitTests, FormHelper formHelper):base(i)
    {
        _getUnitTests = getUnitTests;
        FormHelper = formHelper;

        _header = this
            .WhenAnyValue(e => e.Model.Caption)
            .Select(c => $"{{Test class}}\n{c}")
            .ToProperty(this, e => e.Header);

        this.WhenAnyValue(e => e.Model)
            .Select(async e => await Init().ConfigureAwait(true))
            .Subscribe();

        _unitTests = this
            .WhenAnyValue(e => e.Model)
            .Select(e => SetUnitTests())
            .ToProperty(this, e => e.UnitTests);

        TryCommand = ReactiveCommand.CreateFromTask(
            TryAsync,
            this.WhenAnyValue(e => e.FormHelper.FormUpToDate).Select(e => !e)
            );

        FormatCommand = ReactiveCommand.CreateFromTask(
            async () => await FormHelper.FormatAsync().ConfigureAwait(true)
        );

        SpecificationModeCommand = ReactiveCommand.Create(() => FormHelper.Form.Mode = FormMode.Specification);

        CaptureModeCommand = ReactiveCommand.Create(() => FormHelper.Form.Mode = FormMode.Capture);

        AddUnitTestCommand = ReactiveCommand.CreateFromTask(
            AddUnitTestAsync,
            this.WhenAnyValue(e => e.Locker.IsActive)
        );

        CheckUnitTestsCommand = ReactiveCommand.CreateFromTask(CheckUnitTestsAsync);
    }

    async Task Init()
    {
            if (Model.Code != null)
            {
                await FormHelper.ExtractCodeAsync(Model.Code).ConfigureAwait(true);
            }
            else
            {
                FormHelper.Xaml = "<Grid></Grid>";
                FormHelper.Cs = @"
                        using System;
                        using System.Windows;
                        using System.Windows.Controls;
                        using System.Linq;
                        using System.Collections.Generic;
                        namespace Lims
                        {
                            public class Test
                            {
                                public void Process(object sender, RoutedEventArgs e)
                                {
                                }      
                            }
                        }";
            }

            await FormHelper.CompileAsync();

    }


    #region Imports

    readonly Func<TestClass, TestClassUnitTestListViewModel> _getUnitTests;
    public FormHelper FormHelper { get; }

    #endregion


    #region Properties

    /// <summary>
    /// Name to be used to create a unit test
    /// </summary>
    public string NewName
    {
        get => _newName;
        set => this.SetAndRaise(ref _newName,value);
    }

    string _newName ;



    /// <summary>
    /// List of unit tests
    /// </summary>
    public TestClassUnitTestListViewModel UnitTests => _unitTests.Value;
    readonly ObservableAsPropertyHelper<TestClassUnitTestListViewModel> _unitTests;

    TestClassUnitTestListViewModel SetUnitTests()
    {
        var vm =  _getUnitTests(Model);
        vm.SetSelectAction(async r =>
        {
            var u = (r==null)?new TestClassUnitTestClone():r.Clone<TestClassUnitTestClone>();
            await LoadResultAsync(u).ConfigureAwait(false);
            NewName = u.Name;
            // TODO Reactiveui : if(vm.DeleteCommand is INotifyCommand n) n.CheckCanExecute();
        });

        if(vm.List.Any())
            vm.Selected = vm.List.First();

        return vm;
    }

    #endregion


    #region Commands

    /// <summary>
    /// Try to recompile the form
    /// </summary>
    public ICommand TryCommand { get; }

    async Task TryAsync()
    {
        await FormHelper.CompileAsync();
        Model.Code = await FormHelper.PackCodeAsync();
        Model.Binary = FormHelper.Binary;
    }

    public ICommand FormatCommand { get; }

    /// <summary>
    /// Try to recompile the form
    /// </summary>
    public ICommand SpecificationModeCommand { get; }
    public ICommand CaptureModeCommand { get; }

    /// <summary>
    /// Add unit test form current form state
    /// </summary>
    public ICommand AddUnitTestCommand { get; }

    async Task AddUnitTestAsync()
    {
        await Injected.Data.AddAsync<TestClassUnitTest>(u =>
        {
            u.TestClass = Model;
            u.Name = NewName;

            u.TestName = FormHelper.Form.Target.TestName;
            u.Description = FormHelper.Form.Target.Description;
            u.Specification = FormHelper.Form.Target.Specification;
//                u.SpecificationsDone = FormHelper.Form.Test.;
            u.SpecificationValues = FormHelper.Form.Target.SpecificationValues;
            u.ResultValues = FormHelper.Form.Target.ResultValues;

            u.ConformityId = FormHelper.Form.Target.ConformityId;
            u.Result = FormHelper.Form.Target.Result;
            u.Conformity = FormHelper.Form.Target.Conformity;
            u.MandatoryDone = FormHelper.Form.Target.MandatoryDone;

        });

        UnitTests.List.Update();
    }

    /// <summary>
    /// Check all unit tests
    /// </summary>
    public ICommand CheckUnitTestsCommand { get; }

    async Task CheckUnitTestsAsync()
    {
        foreach (var t in UnitTests.List.ToList())
        {
            var u = t.Clone<TestClassUnitTestClone>();
            await  LoadResultAsync(u).ConfigureAwait(true);
            u.SpecificationValues = FormHelper.Form.Target.SpecificationValues;
            u.ResultValues = FormHelper.Form.Target.ResultValues;

            if(!t.Check(u, out var error)) 
                UnitTests.AddError(t.Id,error);
            else
                UnitTests.AddPassed(t.Id);
        }
        UnitTests.RefreshColumn("error");
    }
    public async Task LoadResultAsync(IFormTarget target=null)
    {
        await FormHelper.LoadAsync(target).ConfigureAwait(true);
        FormHelper.Form.Mode = FormMode.Specification;
    }


    #endregion
}