﻿using System.Windows.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Erp.Lims.Analysis.Samples.SampleTests.SampleTestResults;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Module.SampleTestResults;

/// <summary>
/// Logique d'interaction pour TestView.xaml
/// </summary>
public partial class SampleTestResultDetailView : UserControl, IView<SampleTestResultViewModel>, IDetailViewClass, IDefaultViewClass
{
    public SampleTestResultDetailView()
    {
        InitializeComponent();
    }

}