using System.Windows.Controls;
using HLab.Erp.Lims.Analysis.Samples.SampleTests;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Wpf.Samples.SampleTests;

/// <summary>
/// Logique d'interaction pour SampleTestView.xaml
/// </summary>
public partial class SampleTestView : UserControl, IView<SampleTestViewModel>, IDocumentViewClass
{
    public SampleTestView()
    {
        InitializeComponent();
    }
}