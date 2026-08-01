using Avalonia.Controls;
using HLab.Erp.Core.Tools.Details;
// PharmacopoeiaViewModel vit dans le projet partagé sous un namespace Wpf (résidu)
using HLab.Erp.Lims.Analysis.Wpf.Pharmacopoeias;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;

namespace HLab.Erp.Lims.Analysis.Avalonia.Pharmacopoeias;

public partial class PharmacopoeiaView : UserControl, IView<PharmacopoeiaViewModel>, IDetailViewClass, IDocumentViewClass
{
    public PharmacopoeiaView()
    {
        InitializeComponent();
    }
}
