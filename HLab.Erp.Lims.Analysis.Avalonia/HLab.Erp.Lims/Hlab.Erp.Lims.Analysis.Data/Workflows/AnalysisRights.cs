using HLab.Erp.Acl;

namespace HLab.Erp.Lims.Analysis.Data.Workflows;

public static class AnalysisRights
{
    //Reception
    public static readonly AclRight AnalysisReceptionSign = AclRight
        .Create("{Allow to sign sample reception}");

    public static readonly AclRight AnalysisReceptionCheck = AclRight
        .Create("{Allow to check sample reception}");

    public static readonly AclRight AnalysisReceptionCreate = AclRight
        .Create("{Allow to create sample reception}");

    //Monograph
    public static readonly AclRight AnalysisMonographSign = AclRight
        .Create("{Allow to sign monograph creation}");

    public static readonly AclRight AnalysisMonographValidate = AclRight
        .Create("{Allow to validate monograph}");

    //Analysis
    public static readonly AclRight AnalysisSchedule = AclRight
        .Create("{Allow to schedule analysis}");

    //Tests
    public static readonly AclRight AnalysisAddTest = AclRight
        .Create("{Allow to Add test}");

    public static readonly AclRight AnalysisAddResult = AclRight
        .Create("{Allow to add result}");

    // TODO : fill the rest of the rights
    //Results
    public static readonly AclRight AnalysisResultValidate = AclRight.Create();
    public static readonly AclRight AnalysisResultEnter = AclRight.Create();
    public static readonly AclRight AnalysisResultCheck = AclRight.Create();

    //Certificate
    public static readonly AclRight AnalysisCertificateCreate = AclRight.Create();

    public static readonly AclRight AnalysisAbort = AclRight.Create();

    //Products
    public static readonly AclRight AnalysisProductCreate = AclRight.Create();

    //Manufacturer
    public static readonly AclRight AnalysisManufacturerCreate = AclRight.Create();

    //TestClass
    public static readonly AclRight AnalysisTestClassCreate = AclRight.Create();

    //StatQuery
    public static readonly AclRight AnalysisStatQueryCreate = AclRight.Create();
}