using Avalonia.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Erp.Lims.Analysis.Wpf.TestClasses;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Avalonia.TestClasses;

public partial class TestCategoryDetailView : UserControl, IView<TestCategoryViewModel>, IDetailViewClass, IDocumentViewClass
{
    public TestCategoryDetailView()
    {
        InitializeComponent();
    }
}
