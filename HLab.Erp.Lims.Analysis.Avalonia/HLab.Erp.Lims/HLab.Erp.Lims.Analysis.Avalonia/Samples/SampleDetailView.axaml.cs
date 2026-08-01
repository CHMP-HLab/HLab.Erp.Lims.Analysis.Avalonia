using Avalonia.Controls;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Avalonia.Samples;

/// <summary>
/// Logique d'interaction pour SampleDetailView.axaml
/// </summary>
public partial class SampleDetailView : UserControl, IView<SampleViewModel>
{
    public SampleDetailView()
    {
        InitializeComponent();
    }
}
