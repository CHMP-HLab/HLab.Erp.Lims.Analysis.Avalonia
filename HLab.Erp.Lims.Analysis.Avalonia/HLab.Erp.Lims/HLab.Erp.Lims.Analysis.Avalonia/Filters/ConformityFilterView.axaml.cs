using Avalonia.Controls;
using HLab.Erp.Core.Wpf.ListFilters;
using HLab.Erp.Workflows.Interfaces;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Avalonia.Filters;

public partial class ConformityFilterView : UserControl, IView<DefaultViewMode, IWorkflowFilter>, IFilterContentViewClass
{
    public ConformityFilterView()
    {
        InitializeComponent();
    }

    public void SetFocus()
    {
    }
}
