using Avalonia.Controls;
using HLab.Erp.Lims.Analysis.Samples.SampleTests.SampleTestResults;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Avalonia.Samples.SampleTests.SampleTestResults;

public partial class SampleTestResultView : UserControl, IView<SampleTestResultViewModel>, IDocumentViewClass
{
    public SampleTestResultView()
    {
        InitializeComponent();
    }
}
