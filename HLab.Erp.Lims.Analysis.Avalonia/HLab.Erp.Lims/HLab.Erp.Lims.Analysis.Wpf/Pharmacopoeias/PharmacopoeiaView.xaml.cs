using System.Windows.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Erp.Lims.Analysis.Wpf.Pharmacopoeias;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Module.Pharmacopoeias;

/// <summary>
/// Logique d'interaction pour ProductView.xaml
/// </summary>
public partial class PharmacopoeiaView : UserControl, IView<PharmacopoeiaViewModel>, IDetailViewClass, IDocumentViewClass
{
    public PharmacopoeiaView()
    {
        InitializeComponent();
    }
}