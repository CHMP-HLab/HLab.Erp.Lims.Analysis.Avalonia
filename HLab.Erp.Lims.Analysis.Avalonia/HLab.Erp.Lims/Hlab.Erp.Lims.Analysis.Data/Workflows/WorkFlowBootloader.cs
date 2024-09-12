using System.Threading.Tasks;
using HLab.Core.Annotations;
using HLab.Erp.Acl;

namespace HLab.Erp.Lims.Analysis.Data.Workflows;

public class WorkFlowBootloader : IBootloader
{
    readonly IAclService _acl;

    public WorkFlowBootloader(IAclService acl)
    {
        _acl = acl;
    }

    public Task LoadAsync(IBootContext bootstrapper)
    {
        WorkflowAnalysisExtension.Acl = _acl;
        return Task.CompletedTask;
    }
}