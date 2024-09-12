using System.Windows.Controls;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Wpf.Samples.SampleTests;

internal class SampleTestInlineViewModel : ViewModel<SampleTest>
{

}

/// <summary>
/// Logique d'interaction pour TestView.xaml
/// </summary>
public partial class TestView : UserControl, IView<SampleTestInlineViewModel> , IListElementViewClass
{
    public TestView()
    {
        InitializeComponent();
    }

}