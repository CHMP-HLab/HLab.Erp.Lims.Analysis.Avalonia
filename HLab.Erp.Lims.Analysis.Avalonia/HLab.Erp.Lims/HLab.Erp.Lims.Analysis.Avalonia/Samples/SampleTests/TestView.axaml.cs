using Avalonia.Controls;
using HLab.Erp.Lims.Analysis.Samples.SampleTests;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Avalonia.Samples.SampleTests;

public partial class TestView : UserControl, IView<ListViewMode, SampleTestViewModel>
{
    public TestView()
    {
        InitializeComponent();
    }
}
