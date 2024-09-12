using System.Windows.Controls;
using HLab.Erp.Lims.Analysis.Samples;
using HLab.Erp.Lims.Analysis.Wpf.Samples;
using HLab.Erp.Workflows;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Module.Samples;

/// <summary>
/// Logique d'interaction pour SampleView.xaml
/// </summary>
public partial class SampleView : UserControl , IView<SampleViewModel>, IDocumentViewClass
{
    public SampleView()
    {
        InitializeComponent();

        this.SetHighlights(vm=>vm.Workflow);
    }
}