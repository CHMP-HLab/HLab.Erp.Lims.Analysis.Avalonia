using Avalonia.Controls;
using HLab.Erp.Core.Wpf.ListFilters;
using HLab.Erp.Workflows.Interfaces;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Workflow.Avalonia.Views
{
    /// <summary>
    /// Logique d'interaction pour FilterEntityView.xaml
    /// </summary>
    public partial class WorkflowFilterView : UserControl , IView<DefaultViewMode, IWorkflowFilter>, IFilterContentViewClass
    {
        public WorkflowFilterView()
        {
            InitializeComponent();
        }

        public void SetFocus()
        {
        }
    }
}
