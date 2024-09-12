using System.Windows.Controls;
using HLab.Erp.Lims.Analysis.Module.SampleTestResults;
using HLab.Erp.Lims.Analysis.Samples.SampleTests.SampleTestResults;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Module.SampleTests;

/// <summary>
/// Logique d'interaction pour SampleTestView.xaml
/// </summary>
public partial class SampleTestResultView : UserControl, IView<SampleTestResultViewModel>, IDocumentViewClass
{
    public SampleTestResultView()
    {
        InitializeComponent();
    }
}