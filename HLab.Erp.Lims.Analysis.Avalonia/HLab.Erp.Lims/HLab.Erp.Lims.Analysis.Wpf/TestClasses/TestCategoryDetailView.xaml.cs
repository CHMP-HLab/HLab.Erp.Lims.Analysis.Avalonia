using System.Windows.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Wpf.TestClasses;

/// <summary>
/// Logique d'interaction pour TestView.xaml
/// </summary>
public partial class TestCategoryDetailView : UserControl, IView<TestCategoryViewModel>, IDetailViewClass, IDocumentViewClass
{
    public TestCategoryDetailView()
    {
        InitializeComponent();
    }
}