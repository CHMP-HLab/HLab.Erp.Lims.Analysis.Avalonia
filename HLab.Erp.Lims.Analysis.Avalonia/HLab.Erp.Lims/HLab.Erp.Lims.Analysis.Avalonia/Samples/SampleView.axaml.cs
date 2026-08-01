using Avalonia.Controls;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Avalonia.Samples;

/// <summary>
/// Logique d'interaction pour SampleView.axaml
/// </summary>
public partial class SampleView : UserControl, IView<SampleViewModel>, IDocumentViewClass
{
    public SampleView()
    {
        InitializeComponent();

        // TODO : SetHighlights (HighlightHelper de HLab.Erp.Workflows.Wpf non porté)
    }
}
