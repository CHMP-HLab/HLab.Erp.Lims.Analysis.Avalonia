using Avalonia.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Erp.Lims.Analysis.Products;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Avalonia.Products.Views;

/// <summary>
/// Logique d'interaction pour ProductView.axaml
/// </summary>
public partial class ProductView : UserControl, IView<ProductViewModel>, IDetailViewClass, IDocumentViewClass
{
    public ProductView()
    {
        InitializeComponent();
    }
}
