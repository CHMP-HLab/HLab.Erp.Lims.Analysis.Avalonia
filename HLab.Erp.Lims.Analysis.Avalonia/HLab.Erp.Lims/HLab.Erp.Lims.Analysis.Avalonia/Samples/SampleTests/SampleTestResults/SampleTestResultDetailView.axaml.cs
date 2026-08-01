using Avalonia.Controls;
using HLab.Erp.Core.Tools.Details;
using HLab.Erp.Lims.Analysis.Samples.SampleTests.SampleTestResults;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Avalonia.Samples.SampleTests.SampleTestResults;

public partial class SampleTestResultDetailView : UserControl, IView<SampleTestResultViewModel>, IDetailViewClass
{
    public SampleTestResultDetailView()
    {
        InitializeComponent();
    }
}
