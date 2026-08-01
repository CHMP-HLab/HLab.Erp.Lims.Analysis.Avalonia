using Avalonia.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Erp.Lims.Analysis.TestClasses;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Avalonia.TestClasses;

public partial class TestClassDetailView : UserControl, IView<TestClassViewModel>, IDetailViewClass, IDocumentViewClass
{
    public TestClassDetailView()
    {
        InitializeComponent();
    }
}
