using HLab.Mvvm.Annotations;
using System.Windows.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Erp.Lims.Analysis.Module.Samples;
using HLab.Erp.Lims.Analysis.Samples;
using HLab.Erp.Lims.Analysis.Wpf.Samples;

namespace HLab.Erp.Lims.Analysis.Module;

/// <summary>
/// Logique d'interaction pour SampleDetailView.xaml
/// </summary>
public partial class SampleDetailView : UserControl , IView<SampleViewModel>, IDetailViewClass

{
    public SampleDetailView()
    {
        InitializeComponent();
    }

    void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}