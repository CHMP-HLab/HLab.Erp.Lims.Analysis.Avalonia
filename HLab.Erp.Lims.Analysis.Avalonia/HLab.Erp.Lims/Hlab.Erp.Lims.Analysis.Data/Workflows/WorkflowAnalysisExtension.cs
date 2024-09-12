using System;
using System.Text;
using HLab.Base.Fluent;
using HLab.Base.ReactiveUI;
using HLab.Erp.Acl;
using HLab.Erp.Workflows.Extensions;
using HLab.Erp.Workflows.Interfaces;
using HLab.Erp.Workflows.Models;

namespace HLab.Erp.Lims.Analysis.Data.Workflows;

public static class WorkflowAnalysisExtension
{
    public static IAclService Acl { get; set; }
    public static IFluentConfigurator<IWorkflowConditionalObject<TWf>> NeedRight<TWf>(this IFluentConfigurator<IWorkflowConditionalObject<TWf>> t, Func<AclRight> right)
        where TWf : ReactiveModel, IWorkflow<TWf>
    {
        return t.When(w => Acl.IsGranted(
                right(),
                w.User,w.Target))
            .WithMessage(w => "{Not allowed} {need} " + right().Caption);
    }
    public static IFluentConfigurator<IWorkflowConditionalObject<TWf>> NeedAnyRight<TWf>(this IFluentConfigurator<IWorkflowConditionalObject<TWf>> t, params Func<AclRight>[] rights)
        where TWf : ReactiveModel, IWorkflow<TWf> => t.When(w =>
                                                              {
                                                                  foreach (var right in rights)
                                                                      if (Acl.IsGranted(right(), w.User, w.Target)) return true;
                                                                  return false;
                                                              })
        .WithMessage(w =>
        {
            var s = new StringBuilder("{Not allowed} {need} ");
            foreach (var right in rights) s.Append(right().Caption).Append(" ");
            return  s.ToString();
        });

    public static IFluentConfigurator<IWorkflowConditionalObject<TWf>> NeedPharmacist<TWf>(this IFluentConfigurator<IWorkflowConditionalObject<TWf>> t)
        where TWf : ReactiveModel, IWorkflow<TWf>
    {
        return t.NotWhen(w => !Acl.IsGranted(
                AnalysisRights.AnalysisCertificateCreate,
                w.User,w.Target))
            .WithMessage(w => "{Pharmacist needed}");
    }

    public static IFluentConfigurator<IWorkflowConditionalObject<TWf>> NeedValidator<TWf>(this IFluentConfigurator<IWorkflowConditionalObject<TWf>> t)
        where TWf : Workflow<TWf>
    {
        return t.NotWhen(w => !Acl.IsGranted(AnalysisRights.AnalysisResultValidate
            ,w.User,w.Target))
            .WithMessage(w => "validator needed");
    }

    public static IFluentConfigurator<IWorkflowConditionalObject<TWf>> NeedPlanner<TWf>(this IFluentConfigurator<IWorkflowConditionalObject<TWf>> t)
        where TWf : ReactiveModel, IWorkflow<TWf>
    {
        return t.NotWhen(w => !Acl.IsGranted(AnalysisRights.AnalysisSchedule
            ,w.User,w.Target))
            .WithMessage(w => "planner needed");
    }

    public static IFluentConfigurator<IWorkflowConditionalObject<TWf>> 
        SetStage<TWf>(this IFluentConfigurator<IWorkflowConditionalObject<TWf>> t, Func<Workflow<TWf>.Stage> getter)
        where TWf : ReactiveModel, IWorkflow<TWf>
    {
        return t
            .Action(async w =>
            {
                await w.SetStageAsync(getter,"","",false,false);
            });
    }
}