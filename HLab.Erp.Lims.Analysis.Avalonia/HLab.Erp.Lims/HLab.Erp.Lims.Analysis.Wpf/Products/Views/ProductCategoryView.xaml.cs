using System.Windows.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Erp.Lims.Analysis.Module.Products.ViewModels;
using HLab.Erp.Lims.Analysis.Products;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Wpf.Products.Views;

/// <summary>
/// Logique d'interaction pour ProductView.xaml
/// </summary>
public partial class ProductCategoryView : UserControl
    , IView<ProductCategoryViewModel>
    , IDetailViewClass
    , IDocumentViewClass
{
    public ProductCategoryView()
    {
        InitializeComponent();
    }
}