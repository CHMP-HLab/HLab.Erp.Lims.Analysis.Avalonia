using Avalonia.Controls;
using HLab.Erp.Lims.Analysis.Products.Tools;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Avalonia.Products.Tools;

public partial class ProductToolsView : UserControl, IView<ProductToolsViewModel>, IDocumentViewClass
{
    public ProductToolsView()
    {
        InitializeComponent();
    }
}
