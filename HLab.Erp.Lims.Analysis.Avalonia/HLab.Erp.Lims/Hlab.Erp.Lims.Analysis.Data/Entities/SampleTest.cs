using System;
using HLab.Base.ReactiveUI;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using NPoco;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public partial class SampleTest : Entity
    , IEntityWithIcon
    , IEntityWithColor
    , IFormTarget
{

    public SampleTest()
    {
        _sample = Foreign(this, e => e.SampleId, e => e.Sample);
        _testClass = Foreign(this, e => e.TestClassId, e => e.TestClass);
        _result = Foreign(this, e => e.ResultId, e => e.Result);
        _pharmacopoeia = Foreign(this, e => e.PharmacopoeiaId, e => e.Pharmacopoeia);
        _productComponent = Foreign(this, e => e.ProductComponentId, e => e.ProductComponent);

        _color = this
            .WhenAnyValue(e => e.TestClass.Color)
            .ToProperty(this, e => e.Color);

        _stage = this
            .WhenAnyValue(e => e.StageId, id => SampleTestWorkflow.StageFromName(id))
            .ToProperty(this, e => e.Stage);

        _iconPath = this
            .WhenAnyValue(e => e.TestClass.IconPath, iconPath => string.IsNullOrWhiteSpace(iconPath) ? "icon/test/default" : iconPath)
            .ToProperty(this, e => e.IconPath);
    }

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
    readonly ForeignPropertyHelper<SampleTest, Sample> _sample;


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
    readonly ForeignPropertyHelper<SampleTest, TestClass> _testClass;


    public int? TestStateId
    {
        get => _testStateId;
        set => this.SetAndRaise(ref _testStateId, value);
    }

    int? _testStateId;


    public int? UserId
    {
        get => _userId;
        set => this.SetAndRaise(ref _userId, value);
    }

    int? _userId;


    public string Method { get; set; }

    public int? PurposeId
    {
        get => _purposeId;
        set => this.SetAndRaise(ref _purposeId, value);
    }

    int? _purposeId;

    public string Note
    {
        get => _note;
        set => this.SetAndRaise(ref _note, value);
    }

    string _note;

    public int? Validation
    {
        get => _validation;
        set => this.SetAndRaise(ref _validation, value);
    }

    int? _validation;


    public int? ValidatorId
    {
        get => _validatorId;
        set => this.SetAndRaise(ref _validatorId, value);
    }

    int? _validatorId;

    public string TestName
    {
        get => _testName;
        set => this.SetAndRaise(ref _testName, value);
    }

    string _testName = "";


    public string Version
    {
        get => _version;
        set => this.SetAndRaise(ref _version, value);
    }

    string _version = "";


    //public byte[] Code
    //{
    //    get => _code; 
    //    set => this.SetAndRaise(ref _code,value);
    //}
    //private byte[] _code ;
    byte[] IFormTarget.Code => TestClass.Code;


    public string Description
    {
        get => _description;
        set => this.SetAndRaise(ref _description, value);
    }

    string _description = "";




    public string Specification
    {
        get => _specification;
        set => this.SetAndRaise(ref _specification, value);
    }

    string _specification = "";


    public string Values
    {
        get => _values;
        set => this.SetAndRaise(ref _values, value);
    }

    string _values = "";

    string IFormTarget.SpecificationValues
    {
        get => Values;
        set => Values = value;
    }

    public DateTime? ScheduledDate
    {
        get => _scheduledDate;
        set => this.SetAndRaise(ref _scheduledDate, value);
    }

    DateTime? _scheduledDate;

    public DateTime? StartDate
    {
        get => _startDate;
        set => this.SetAndRaise(ref _startDate, value);
    }

    DateTime? _startDate;


    public DateTime? EndDate
    {
        get => _endDate;
        set => this.SetAndRaise(ref _endDate, value);
    }

    DateTime? _endDate;


    public string OosNo
    {
        get => _oosNo;
        set => this.SetAndRaise(ref _oosNo, value);
    }

    string _oosNo = "";

    public int? Order
    {
        get => _order;
        set => this.SetAndRaise(ref _order, value);
    }

    int? _order;


    public string PharmacopoeiaVersion
    {
        get => _pharmacopoeiaVersion;
        set => this.SetAndRaise(ref _pharmacopoeiaVersion, value);
    }

    string _pharmacopoeiaVersion = "";

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
    ForeignPropertyHelper<SampleTest, Pharmacopoeia> _pharmacopoeia;

    public int? ProductComponentId
    {
        get => _productComponent.Id;
        set => _productComponent.SetId(value);
    }
    [Ignore]
    public ProductComponent ProductComponent
    {
        get => _productComponent.Value;
        set => ProductComponentId = value?.Id;
    }
    ForeignPropertyHelper<SampleTest, ProductComponent> _productComponent;

    [Column("Stage")]
    public string StageId
    {
        get => _stageId;
        set => this.SetAndRaise(ref _stageId, value);
    }

    string _stageId;

    [Ignore]
    public SampleTestWorkflow.Stage Stage
    {
        get => _stage.Value;
        set => StageId = value.Name;
    }
    ObservableAsPropertyHelper<SampleTestWorkflow.Stage> _stage;


    public bool SpecificationDone
    {
        get => _specificationDone;
        set => this.SetAndRaise(ref _specificationDone, value);
    }

    bool _specificationDone;

    // RESULT
    public int? ResultId
    {
        get => _result.Id;
        set => _result.SetId(value);
    }
    [Ignore]
    public SampleTestResult Result
    {
        get => _result.Value;
        set => ResultId = value.Id;
    }
    ForeignPropertyHelper<SampleTest, SampleTestResult> _result;

    bool IFormTarget.MandatoryDone
    {
        get => Result?.MandatoryDone ?? true;
        set
        {
            if (Result != null)
                Result.MandatoryDone = value;
        }
    }

    string IFormTarget.Conformity
    {
        get => Result?.Conformity;
        set
        {
            if (Result != null)
                Result.Conformity = value;
        }
    }

    public double Progress
    {
        get => _progress;
        set => this.SetAndRaise(ref _progress, value);
    }

    double _progress;

    [Ignore]
    string IFormTarget.Result
    {
        get => Result?.Result;
        set
        {
            if (Result != null)
                Result.Result = value;
        }
    }

    ConformityState IFormTarget.ConformityId
    {
        get => Result?.ConformityId ?? ConformityState.NotChecked;
        set
        {
            if (Result != null)
                Result.ConformityId = value;
        }
    }

    string IFormTarget.ResultValues
    {
        get => Result?.Values;
        set
        {
            if (Result != null)
                Result.Values = value;
        }
    }

    // CALCULATED

    [Ignore]
    public int? Color => _color.Value;
    readonly ObservableAsPropertyHelper<int?> _color;

    [Ignore]
    public string IconPath => _iconPath.Value;
    ObservableAsPropertyHelper<string> _iconPath;


    [Ignore]
    public ObservableQuery<SampleTestResult> Results;
    // TODO : implement

    //private ObservableQuery<SampleTestResult> _results = H.Property<ObservableQuery<SampleTestResult>>(c => c
    //    .Foreign(e => e.SampleTestId)
    //);


    [Ignore] string IFormTarget.DefaultTestName => TestClass?.Name;

    IFormClass IFormTarget.FormClass { get => TestClass; set => TestClass = (TestClass)value; }
    string IFormTarget.Name { get => TestClass?.Name; set => throw new NotImplementedException(); }



}
