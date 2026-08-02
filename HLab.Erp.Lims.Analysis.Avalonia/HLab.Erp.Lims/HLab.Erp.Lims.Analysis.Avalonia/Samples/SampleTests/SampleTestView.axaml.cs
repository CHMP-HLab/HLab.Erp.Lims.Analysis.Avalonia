using Avalonia.Controls;
using HLab.Erp.Lims.Analysis.Samples.SampleTests;
using HLab.Erp.Workflows;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Avalonia.Samples.SampleTests;

public partial class SampleTestView : UserControl, IView<SampleTestViewModel>, IDocumentViewClass
{
    public SampleTestView()
    {
        InitializeComponent();

        this.SetHighlights(vm => vm.Workflow);
    }
}
