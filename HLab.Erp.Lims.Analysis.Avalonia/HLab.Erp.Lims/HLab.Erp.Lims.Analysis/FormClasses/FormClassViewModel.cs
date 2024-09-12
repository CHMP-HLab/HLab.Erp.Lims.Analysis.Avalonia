using System.Reactive.Linq;
using System.Windows.Input;
using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Data.Entities;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.FormClasses;

public class FormClassViewModel : ListableEntityViewModel<FormClass>, IFormHelperProvider
{
    public FormClassViewModel(Injector i, FormHelper formHelper, ISampleTestFormClassProvider provider):base(i)
    {
        FormHelper = formHelper;

        this.WhenAnyValue(e => e.Model.Code)
            .Select(async code => await InitAsync(provider, code))
            .Subscribe();

        TryCommand = ReactiveCommand.CreateFromTask(
            async () => await TryAsync(provider), this
            .WhenAnyValue(e => e.FormHelper.FormUpToDate)
            .Select(upToDate => !upToDate)
            );

        SpecificationModeCommand = ReactiveCommand.Create(
            () => FormHelper.Form.Mode = FormMode.Specification
            );

        CaptureModeCommand = ReactiveCommand.Create(
            () => FormHelper.Form.Mode = FormMode.Capture
            );

    }

    async Task InitAsync(ISampleTestFormClassProvider provider, byte[]? code)
    {
        if (Model is not null &&  code is { Length: > 0 })
        {
            await FormHelper.ExtractCodeAsync(Model.Code).ConfigureAwait(true);
        }
        else
        {
            FormHelper.Xaml = "<Grid></Grid>";
            FormHelper.Cs = """ 
                                using System;
                                using System.Windows;
                                using System.Windows.Controls;
                                using Outils;
                                using System.Linq;
                                using System.Collections.Generic;
                                namespace Lims
                                {
                                    public class Form
                                    {
                                        public void Process(object sender, RoutedEventArgs e)
                                        {
                                        }      
                                    }
                                }
                            """;
        }

        await FormHelper.CompileAsync(provider);

    }

    public FormHelper FormHelper { get; }

    /// <summary>
    /// Try to recompile the form
    /// </summary>
    public ICommand TryCommand { get; } 

    async Task TryAsync(ISampleTestFormClassProvider provider)
    {
        if(Model is null) return;
        await FormHelper.CompileAsync(provider);
        Model.Code = await FormHelper.PackCodeAsync();
    }

    /// <summary>
    /// Test specification mode
    /// </summary>
    public ICommand SpecificationModeCommand { get; }

    /// <summary>
    /// Test capture mode
    /// </summary>
    public ICommand CaptureModeCommand { get; }

}