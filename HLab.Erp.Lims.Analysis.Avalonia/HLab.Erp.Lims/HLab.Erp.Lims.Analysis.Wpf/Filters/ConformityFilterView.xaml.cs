using System.Windows.Controls;
using HLab.Erp.Core.Wpf.ListFilters;
using HLab.Erp.Workflows.Interfaces;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Wpf.Filters;

/// <summary>
/// Logique d'interaction pour FilterEntityView.xaml
/// </summary>
public partial class ConformityFilterView : UserControl , IView<DefaultViewMode, IWorkflowFilter>, IFilterContentViewClass
{
    public ConformityFilterView()
    {
        InitializeComponent();
    }

    public void SetFocus()
    {
    }
}