using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Data;
using NPoco;

namespace HLab.Erp.Lims.Analysis.Data;



public class TestClassUnitTest : Entity, IFormTarget
{
    public TestClassUnitTest() {
        _testClass = Foreign(this,e => e.TestClassId, e => e.TestClass);
    }

    public int? TestClassId
    {
        get => _testClass.Id;
        set => _testClass.SetId(value);
    }
    [Ignore] public TestClass TestClass
    {
        get => _testClass.Value;
        set => TestClassId = value.Id;
    }
    ForeignPropertyHelper<TestClassUnitTest,TestClass> _testClass;
    [Ignore] IFormClass IFormTarget.FormClass 
    {
        get => TestClass;
        set => TestClass = (TestClass)value;
    }

    [Ignore] byte[] IFormTarget.Code => TestClass?.Code;
    [Ignore] string IFormTarget.DefaultTestName => TestClass?.Name;

    public string Name
    {
        get => _name;
        set => SetAndRaise(ref _name,value);
    }

    private string _name = "";
    public string ResultValues
    {
        get => _resultValues;
        set => SetAndRaise(ref _resultValues,value);
    }
    private string _resultValues = "";

    public string SpecificationValues
    {
        get => _specificationValues;
        set => SetAndRaise(ref _specificationValues,value);
    }
    private string _specificationValues = "";

    public string TestName
    {
        get => _testName;
        set => SetAndRaise(ref _testName,value);
    }

    private string _testName = "";

    public string Description
    {
        get => _description;
        set => SetAndRaise(ref _description,value);
    }
    private string _description = "";

    public string Specification
    {
        get => _specification;
        set => SetAndRaise(ref _specification,value);
    }
    private string _specification = "";
    
    public bool SpecificationDone
    {
        get => _specificationDone;
        set => SetAndRaise(ref _specificationDone,value);
    }
    private bool _specificationDone = false;

    public string Result
    {
        get => _result;
        set => SetAndRaise(ref _result,value);
    }
    private string _result = "";

    public ConformityState ConformityId
    {
        get => _conformityId;
        set => SetAndRaise(ref _conformityId,value);
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    private ConformityState _conformityId = ConformityState.NotChecked;

    public bool MandatoryDone
    {
        get => _mandatoryDone;
        set => SetAndRaise(ref _mandatoryDone,value);
    }
    private bool _mandatoryDone = false;

    public string Conformity
    {
        get => _conformity;
        set => SetAndRaise(ref _conformity,value);
    }
    private string _conformity = "";


    //public string Conform
    //{
    //    get => _conform;
    //    set => SetAndRaise(ref _conform,value);
    //}
    //private string _conform = "";

    //public string State
    //{
    //    get => _state;
    //    set => SetAndRaise(ref _state,value);
    //}
    //private string _state = "";
}
