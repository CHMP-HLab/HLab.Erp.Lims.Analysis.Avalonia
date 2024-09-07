using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Workflows;

namespace HLab.Erp.Lims.Analysis.Data.Workflows;

public class ProductWorkflow : Workflow<ProductWorkflow, Product>
{
    public ProductWorkflow(Product product, IDataLocker locker) : base(product, locker)
    {
        CurrentStage = Created;
        Update();
    }

    public static Stage Created = Stage.Create(c => c
        .Caption("^Reception entry").Icon("Icons/Sample/PackageOpened")
            .SetStage(() => Created)
    );

    protected override Stage TargetStage { get; set; }
}