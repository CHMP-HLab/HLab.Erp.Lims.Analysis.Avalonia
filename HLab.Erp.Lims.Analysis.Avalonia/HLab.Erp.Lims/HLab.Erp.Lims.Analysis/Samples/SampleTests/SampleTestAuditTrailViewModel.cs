using System;
using HLab.Erp.Acl.AuditTrails;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Lims.Analysis.Data.Workflows;

namespace HLab.Erp.Lims.Analysis.Wpf.Samples.SampleTests;

public class SampleTestAuditTrailViewModel(EntityListViewModel<AuditTrail>.Injector i, int sampleTestId)
    : Core.EntityLists.EntityListViewModel<AuditTrail>(i, c => c
        .StaticFilter(e => e.EntityId == sampleTestId)
        .StaticFilter(e => e.EntityClass == "SampleTest")
        .Column("Stage")
        .Header("{Stage}").Width(150).Localize(at => $"{GetStage(at.Log)}")
        .Icon(at => at.IconPath, 20)
        .Column("Date")
        .Header("{Date}").Width(110).Content(at => at.TimeStamp)
        .Column("User")
        .Header("{User}").Width(150).Content(at => at.UserCaption)
        .Column("Motivation")
        .Header("{Motivation}").Width(250)
        .Content(at => at.Motivation)
        .Column("Log")
        .Header("{Log}").Width(150).Localize(at => $"{at.Log}"))
{
    static string GetStage(string log)
    {
        var lines = log.Replace("\r","").Split('\n');
        foreach (var line in lines)
        {
            var part = line.Split('=');

            if(part.Length>1)
            {
                switch(part[0])
                {
                    case "Stage":
                    case "StageId":
                        return SampleTestWorkflow.StageFromName(part[1]).GetCaption(null);
                }
            }
        }
        return "NA";
    }

    string LogAbstract(string log, int size)
    {
        const string suffix = "...";

        var result = log.Replace('\n', '/').Replace("\r","");
        if (result.Length < size) return result;
        result = result.Substring(0, Math.Max(0,size - suffix.Length)) + suffix;
        return result;
    }
}