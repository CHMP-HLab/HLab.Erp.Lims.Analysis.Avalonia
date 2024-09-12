using System.Windows.Controls;
using HLab.Erp.Lims.Analysis.Products.Tools;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Wpf.Products.Tools;

/// <summary>
/// Logique d'interaction pour ImportUsers.xaml
/// </summary>
public partial class ProductToolsView : UserControl, IView<ProductToolsViewModel>, IDocumentViewClass
{
    public ProductToolsView()
    {
        InitializeComponent();
    }
}