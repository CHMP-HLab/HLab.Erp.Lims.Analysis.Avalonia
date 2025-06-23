using HLab.Core.Annotations;
using HLab.Erp.Acl;

namespace HLab.Erp.Lims.Analysis.Data.Workflows;

public class WorkFlowBootloader(IAclService acl) : Bootloader
{
   protected override BootState Load()
   {
      WorkflowAnalysisExtension.Acl = acl;
      return base.Load();
   }
}