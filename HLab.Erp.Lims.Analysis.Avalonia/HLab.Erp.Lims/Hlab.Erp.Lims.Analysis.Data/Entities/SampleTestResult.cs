using System;
using System.Reactive.Linq;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Data;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using NPoco;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data.Entities;




public partial class SampleTestResult : Entity, IFormTarget
//        , IEntityWithIcon
//        , IEntityWithColor
{
    public SampleTestResult() {
        // = H.Property<SampleTestResultWorkflow.Stage>(c => c
        //.Set(e => SampleTestResultWorkflow.StageFromName(e.StageId))
        //.On(e => e.StageId)
        //.Update()
        _stage = this.WhenAnyValue(e => e.StageId)
            .Select(SampleTestResultWorkflow.StageFromName)
            .ToProperty(this, e => e.Stage);

    }

    //public int? Color => _color.Get();
    //private int? _color = H.Property<int?>(c => c.OneWayBind(e => e.SampleTest.TestClass.Color));

    //public string IconName => _iconName.Get();
    //private string _iconName = H.Property<string>(c => c.OneWayBind(e => e.SampleTest.TestClass.IconName));

    public int? SampleTestId
    {
        get => _sampleTest.Id;
        set => _sampleTest.SetId(value);
    }

    [Ignore]
    public virtual SampleTest SampleTest
    {
        get => _sampleTest.Value;
        set => SampleTestId = value?.Id;
    }
    ForeignPropertyHelper<SampleTestResult, SampleTest> _sampleTest;

    public int? UserId
    {
        get => _userId;
        set => SetAndRaise(ref _userId, value);
    }

    private int? _userId;

    public string Values
    {
        get => _values;
        set => SetAndRaise(ref _values, value);
    }
    private string _values = "";

    public string Result
    {
        get => _result;
        set => SetAndRaise(ref _result, value);
    }
    private string _result = "";


    public string Conformity
    {
        get => _conformity;
        set => SetAndRaise(ref _conformity, value);
    }
    private string _conformity = "";

    public DateTime? Start
    {
        get => _start;
        set => SetAndRaise(ref _start, value);
    }
    private DateTime? _start;

    public DateTime? End
    {
        get => _end;
        set => SetAndRaise(ref _end, value);
    }

    private DateTime? _end;


    public ConformityState ConformityId
    {
        get => _conformityId;
        set
        {
#if DEBUG
            if (_conformityId != default && _conformityId != value)
            {
                if (Stage != SampleTestResultWorkflow.Running)
                {
                    throw new Exception("Invalid state");
                }
            }
#endif
            _conformityId = value;
        }
    }

    private ConformityState _conformityId;


    public int? Validation
    {
        get => _validation;
        set => SetAndRaise(ref _validation, value);
    }
    private int? _validation;

    [Column("Stage")]
    public string StageId
    {
        get => _stageId;
        set => SetAndRaise(ref _stageId, value);
    }
    private string _stageId;

    [Ignore]
    public SampleTestResultWorkflow.Stage Stage
    {
        get => _stage.Value;
        set => StageId = value.Name;
    }
    ObservableAsPropertyHelper<SampleTestResultWorkflow.Stage> _stage;

    public string Name
    {
        get => _name;
        set => SetAndRaise(ref _name, value);
    }
    private string _name;


    string IFormTarget.ResultValues
    {
        get => Values;
        set => Values = value;
    }

    public bool MandatoryDone
    {
        get => _mandatoryDone;
        set => SetAndRaise(ref _mandatoryDone, value);
    }

    private bool _mandatoryDone;
    public string Note
    {
        get => _note;
        set => SetAndRaise(ref _note, value);
    }
    private string _note;

    public double Progress
    {
        get => _progress;
        set => SetAndRaise(ref _progress, value);
    }
    private double _progress;

    // TEST

    [Ignore]
    string IFormTarget.Description
    {
        get => SampleTest.Description;
        set => SampleTest.Description = value;
    }

    [Ignore]
    string IFormTarget.TestName
    {
        get => SampleTest.TestName;
        set => SampleTest.TestName = value;
    }
    byte[] IFormTarget.Code => SampleTest.TestClass.Code;

    string IFormTarget.SpecificationValues
    {
        get => SampleTest.Values;
        set => SampleTest.Values = value;
    }

    bool IFormTarget.SpecificationDone
    {
        get => SampleTest.SpecificationDone;
        set => SampleTest.SpecificationDone = value;
    }
    string IFormTarget.Specification
    {
        get => SampleTest.Specification;
        set => SampleTest.Specification = value;
    }

    string IFormTarget.DefaultTestName => ((IFormTarget)SampleTest).DefaultTestName;

    public IFormClass FormClass { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}
