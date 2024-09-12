using HLab.Base.ReactiveUI;
using HLab.Erp.Conformity.Annotations;

namespace HLab.Erp.Lims.Analysis.FormClasses;

public class DummyTarget : ReactiveModel, IFormTarget
{
    public string Result
    {
        get => _result;
        set => SetAndRaise(ref _result,value);
    }
    string _result;

    public ConformityState ConformityId
    {
        get => _conformityId; 
        set => SetAndRaise(ref _conformityId,value); 
    }

    ConformityState _conformityId;

    public byte[] Code => null;

    public string SpecificationValues
    {
        get => _specificationValues; 
        set => SetAndRaise(ref _specificationValues,value);
    }

    string _specificationValues;

    public bool SpecificationDone
    {
        get => _specificationDone; 
        set => SetAndRaise(ref _specificationDone,value);
    }

    bool _specificationDone;

    public string ResultValues
    {
        get => _resultValues; 
        set => SetAndRaise(ref _resultValues,value);
    }

    string _resultValues;

    public bool MandatoryDone
    {
        get => _mandatoryDone; 
        set => SetAndRaise(ref _mandatoryDone,value);
    }

    bool _mandatoryDone;


    public string DefaultTestName => "Dummy";
    public string TestName
    {
        get => _testName;
        set => SetAndRaise(ref _testName,value);
    }

    string _testName;

    public string Description
    {
        get => _description;
        set => SetAndRaise(ref _description,value);
    }

    string _description;

    public string Specification
    {
        get => _specification;
        set => SetAndRaise(ref _specification,value);
    }

    string _specification;

    public string Conformity
    {
        get => _conformity;
        set => SetAndRaise(ref _conformity,value);
    }
    string _conformity;

    IFormClass IFormTarget.FormClass { get => null; set => throw new System.InvalidOperationException($"Setting {nameof(IFormTarget.FormClass)} not allowed"); }

    public string Name
    {
        get => _name;
        set => SetAndRaise(ref _name,value);
    }

    string _name = "Dummy";
}