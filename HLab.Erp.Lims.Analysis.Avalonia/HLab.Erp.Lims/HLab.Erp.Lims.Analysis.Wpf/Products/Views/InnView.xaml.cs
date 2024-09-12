using System.Windows.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Erp.Lims.Analysis.Products;
using HLab.Erp.Lims.Analysis.Wpf.Products.ViewModels;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Wpf.Products.Views;

/// <summary>
/// Logique d'interaction pour ProductView.xaml
/// </summary>
public partial class InnView : UserControl, IView<InnViewModel>, IDetailViewClass, IDocumentViewClass
{
    public InnView()
    {
        InitializeComponent();
    }
}