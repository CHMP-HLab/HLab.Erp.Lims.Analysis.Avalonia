using System.Windows.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Erp.Lims.Analysis.Samples.SampleMovements;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Wpf.Samples.SampleMovements;

/// <summary>
/// Logique d'interaction pour ProductView.xaml
/// </summary>
public partial class SampleMovementView : UserControl, IView<SampleMovementViewModel>, IDetailViewClass, IDocumentViewClass
{
    public SampleMovementView() => InitializeComponent();
}