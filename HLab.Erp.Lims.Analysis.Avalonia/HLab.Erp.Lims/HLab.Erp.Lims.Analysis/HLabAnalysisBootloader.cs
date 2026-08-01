using HLab.Core.Annotations;
using HLab.Erp.Base.Data;
using HLab.Erp.Base.Wpf.Entities.Customers;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Manufacturers;
using HLab.Mvvm.Annotations;
using HLab.Mvvm.Application.Documents;
using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Options;

namespace HLab.Erp.Lims.Analysis;

public class HLabAnalysisBootloader(IMvvmService mvvm, IAclService acl, IOptionsService options) : Bootloader
{
   public override async Task<BootState> LoadAsync()
   {
      if (!mvvm.IsPlatformRegistered) return BootState.Requeue;
         
      options.OptionsPath = "HLab.Erp";
         
      _ = SampleWorkflow.Production;
      _ = SampleTestWorkflow.ValidatedResults;
      _ = SampleTestResultWorkflow.Checked;
         
      mvvm.Register(typeof(Customer), typeof(CustomerViewModel), typeof(IDocumentViewClass), typeof(DefaultViewMode));
      mvvm.Register(typeof(Manufacturer), typeof(ManufacturerViewModel), typeof(IDocumentViewClass), typeof(DefaultViewMode));
      
      return BootState.Completed;
   }
}