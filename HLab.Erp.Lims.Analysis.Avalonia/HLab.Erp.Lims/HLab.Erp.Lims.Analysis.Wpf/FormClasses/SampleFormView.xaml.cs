using System.Windows.Controls;
using HLab.Erp.Lims.Analysis.Wpf.FormClasses;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Module.FormClasses;

/// <summary>
/// Logique d'interaction pour SampleTestView.xaml
/// </summary>
public partial class SampleFormView : UserControl, IView<SampleFormViewModel>, IDocumentViewClass
{
    public SampleFormView()
    {
        InitializeComponent();
    }
}