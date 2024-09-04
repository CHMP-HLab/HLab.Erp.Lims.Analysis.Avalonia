using System;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using NPoco;

namespace HLab.Erp.Lims.Analysis.Data;

public partial class SampleTest : Entity
    , IEntityWithIcon
    , IEntityWithColor
    ,IFormTarget
{

    public SampleTest() { }

    public int? SampleId
    {
        get => _sample.Id;
        set => _sample.SetId(value);
    }
    [Ignore]
    public virtual Sample Sample
    {
        get => _sample.Value;
        set => SampleId = value.Id;
    }
    ForeignPropertyHelper<SampleTest,Sample> _sample;


    public int? TestClassId
    {
        get => _testClass.Id;
        set => _testClass.SetId(value);
    }
    [Ignore]
    public virtual TestClass TestClass
    {
        get => _testClass.Value;
        set => TestClassId = value.Id;
    }
    ForeignPropertyHelper<Sample,TestClass> _testClass;


    public int? TestStateId
    {
        get => _testStateId;
        set => SetAndRaise(ref _testStateId,value);
    }
    private int? _testStateId ;


    public int? UserId
    {
        get => _userId;
        set => SetAndRaise(ref _userId,value);
    }
    private int? _userId ;


    public string Method { get; set; }

    public int? PurposeId
    {
        get => _purposeId;
        set => SetAndRaise(ref _purposeId,value);
    }
    private int? _purposeId ;

    public string Note
    {
        get => _note;
        set => SetAndRaise(ref _note,value);
    }
    private string _note ;

    public int? Validation
    {
        get => _validation;
        set => SetAndRaise(ref _validation,value);
    }
    private int? _validation ;


    public int? ValidatorId
    {
        get => _validatorId;
        set => SetAndRaise(ref _validatorId,value);
    }
    private int? _validatorId ;

    public string TestName
    {
        get => _testName;
        set => SetAndRaise(ref _testName,value);
    }
    private string _testName = "";


    public string Version
    {
        get => _version;
        set => SetAndRaise(ref _version,value);
    }
    private string _version = "";


    //public byte[] Code
    //{
    //    get => _code; 
    //    set => SetAndRaise(ref _code,value);
    //}
    //private byte[] _code ;
    byte[] IFormTarget.Code => TestClass.Code;


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


    public string Values
    {
        get => _values;
        set => SetAndRaise(ref _values,value);
    }
    private string _values = "";

    string IFormTarget.SpecificationValues
    {
        get => Values; 
        set => Values = value;
    }

    public DateTime? ScheduledDate
    {
        get => _scheduledDate;
        set => SetAndRaise(ref _scheduledDate,value);
    }
    private DateTime? _scheduledDate ;

    public DateTime? StartDate
    {
        get => _startDate;
        set => SetAndRaise(ref _startDate,value);
    }
    private DateTime? _startDate ;


    public DateTime? EndDate
    {
        get => _endDate;
        set => SetAndRaise(ref _endDate,value);
    }
    private DateTime? _endDate ;


    public string OosNo
    {
        get => _oosNo;
        set => SetAndRaise(ref _oosNo,value);
    }
    private string _oosNo = "";

    public int? Order
    {
        get => _order;
        set => SetAndRaise(ref _order,value);
    }
    private int? _order ;


    public string PharmacopoeiaVersion
    {
        get => _pharmacopoeiaVersion;
        set => SetAndRaise(ref _pharmacopoeiaVersion,value);
    }
    private string _pharmacopoeiaVersion = "";

    public int? PharmacopoeiaId
    {
        get => _pharmacopoeia.Id;
        set => _pharmacopoeia.SetId(value);
    }

    [Ignore]
    public Pharmacopoeia Pharmacopoeia
    {
        get => _pharmacopoeia.Value;
        set => PharmacopoeiaId = value.Id;
    }
    ForeignPropertyHelper<SampleTest,Pharmacopoeia> _pharmacopoeia;

    [Column("Stage")]
    public string StageId
    {
        get => _stageId;
        set => SetAndRaise(ref _stageId,value);
    }
    private string _stageId ;
    
    [Ignore]
    public SampleTestWorkflow.Stage Stage
    {
        get => _stage;
        set => StageId = value.Name;
    }
    private SampleTestWorkflow.Stage _stage = H.Property<SampleTestWorkflow.Stage>(c => c
        .Set(e => SampleTestWorkflow.StageFromName(e.StageId))
        .On(e => e.StageId)
        .Update()
    );
    
    public bool SpecificationDone
    {
        get => _specificationDone;
        set => SetAndRaise(ref _specificationDone,value);
    }
    private bool _specificationDone ;

    // RESULT
    public int? ResultId
    {
        get => _result.Id;
        set => _result.SetId(value);
    }
    [Ignore]public SampleTestResult Result
    {
        get => _result.Value;
        set => ResultId = value.Id;
    }
    ForeignPropertyHelper<SampleTest,SampleTestResult> _result;

    bool IFormTarget.MandatoryDone
    {
        get => Result?.MandatoryDone??true;
        set
        {
            if(Result!=null)
                Result.MandatoryDone = value;
        }
    }

    string IFormTarget.Conformity
    {
        get => Result?.Conformity;
        set
        {
            if(Result!=null)
                Result.Conformity = value;
        }
    }

    public double Progress
    {
        get => _progress;
        set => SetAndRaise(ref _progress,value);
    }
    private double _progress ;

    [Ignore] string IFormTarget.Result
    {
        get => Result?.Result;
        set
        {
            if(Result!=null)
                Result.Result = value;
        }
    }

    ConformityState IFormTarget.ConformityId 
    {
        get => Result?.ConformityId??ConformityState.NotChecked;
        set
        {
            if(Result!=null)
                Result.ConformityId = value;
        }
    }

    string IFormTarget.ResultValues
    {
        get => Result?.Values;
        set
        {
            if(Result!=null)
                Result.Values = value;
        }
    }

    // CALCULATED

    [Ignore]
    public int? Color => _color.Get();
    private int? _color = H.Property<int?>(c => c
        .On(e => e.TestClass.Color)
        .Set(e => e.TestClass?.Color)
    );


    [Ignore]
    public string IconPath => _iconPath.Get();
    private string _iconPath = H.Property<string>(c => c
        .Set(e => string.IsNullOrWhiteSpace(e.TestClass?.IconPath) ? "icon/test/default" : e.TestClass.IconPath)
        .On(e => e.TestClass.IconPath).Update()
    );

    [Ignore]
    public ObservableQuery<SampleTestResult> Results => _results.Get();
    private ObservableQuery<SampleTestResult> _results = H.Property<ObservableQuery<SampleTestResult>>(c => c
        .Foreign(e => e.SampleTestId)
    );


    [Ignore] string IFormTarget.DefaultTestName => TestClass?.Name;

    IFormClass IFormTarget.FormClass { get => TestClass; set => TestClass = (TestClass)value; }
    string IFormTarget.Name { get => TestClass?.Name; set => throw new NotImplementedException(); }

    public void Reset()
    {
        throw new NotImplementedException();
    }


}
