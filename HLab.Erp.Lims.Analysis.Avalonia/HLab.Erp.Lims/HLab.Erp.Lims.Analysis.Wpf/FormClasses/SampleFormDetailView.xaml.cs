using System.Windows.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Wpf.FormClasses;

/// <summary>
/// Logique d'interaction pour TestView.xaml
/// </summary>
public partial class SampleFormDetailView : UserControl, IView<SampleFormViewModel>, IDetailViewClass, IDefaultViewClass
{
    public SampleFormDetailView()
    {
        InitializeComponent();
    }
}