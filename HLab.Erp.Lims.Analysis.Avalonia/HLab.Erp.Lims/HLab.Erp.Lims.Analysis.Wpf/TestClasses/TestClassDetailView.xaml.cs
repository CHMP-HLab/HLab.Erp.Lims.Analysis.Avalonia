using System.Windows.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Erp.Lims.Analysis.TestClasses;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Wpf.TestClasses;

/// <summary>
/// Logique d'interaction pour TestView.xaml
/// </summary>
public partial class TestClassDetailView : UserControl, IView<TestClassViewModel>, IDetailViewClass
{
    public TestClassDetailView()
    {
        InitializeComponent();
    }
}