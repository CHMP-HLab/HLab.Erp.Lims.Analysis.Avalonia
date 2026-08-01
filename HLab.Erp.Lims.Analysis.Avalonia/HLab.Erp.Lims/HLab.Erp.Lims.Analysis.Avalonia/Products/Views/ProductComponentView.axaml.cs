using Avalonia.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Erp.Lims.Analysis.Products;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Avalonia.Products.Views;

public partial class ProductComponentView : UserControl, IView<ProductComponentViewModel>, IDetailViewClass, IDocumentViewClass
{
    public ProductComponentView()
    {
        InitializeComponent();
    }
}
