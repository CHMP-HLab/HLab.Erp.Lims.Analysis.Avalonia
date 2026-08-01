using Avalonia.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Erp.Lims.Analysis.Samples.SampleTests;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Avalonia.Samples.SampleTests;

public partial class SampleTestDetailView : UserControl, IView<SampleTestViewModel>, IDetailViewClass
{
    public SampleTestDetailView()
    {
        InitializeComponent();
    }
}
