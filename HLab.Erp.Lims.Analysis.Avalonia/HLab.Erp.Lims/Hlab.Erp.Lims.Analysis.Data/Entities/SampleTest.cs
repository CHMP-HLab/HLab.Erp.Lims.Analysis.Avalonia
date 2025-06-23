using System;
using HLab.Base.ReactiveUI;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Data;
using HLab.Erp.Data.foreigners;
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
      _sample = this.Foreign(e => e.SampleId, e => e.Sample);
      _testClass = this.Foreign(e => e.TestClassId, e => e.TestClass);
      _result = this.Foreign(e => e.ResultId, e => e.Result);
      _pharmacopoeia = this.Foreign(e => e.PharmacopoeiaId, e => e.Pharmacopoeia);
      _productComponent = this.Foreign(e => e.ProductComponentId, e => e.ProductComponent);

      _color = this
          .WhenAnyValue(e => e.TestClass.Color)
          .ToProperty(this, e => e.Color);

      _stage = this
          .WhenAnyValue(e => e.StageId, SampleTestWorkflow.StageFromName)
          .ToProperty(this, e => e.Stage);

      _iconPath = this
          .WhenAnyValue(e => e.TestClass.IconPath, iconPath => string.IsNullOrWhiteSpace(iconPath) ? "icon/test/default" : iconPath)
          .ToProperty(this, e => e.IconPath);
   }

   public int? SampleId { get => _sample.Id; set => _sample.SetId(value); }
   [Ignore] public virtual Sample? Sample { get => _sample.Value; set => SampleId = value.Id; }
   readonly ForeignPropertyHelper<SampleTest, Sample?> _sample;


   public int? TestClassId { get => _testClass.Id; set => _testClass.SetId(value); }
   [Ignore] public virtual TestClass TestClass { get => _testClass.Value; set => TestClassId = value.Id; }
   readonly ForeignPropertyHelper<SampleTest, TestClass> _testClass;


   public int? TestStateId { get; set => this.SetAndRaise(ref field, value); }

   public int? UserId { get; set => this.SetAndRaise(ref field, value); }


   public string Method { get; set; }

   public int? PurposeId { get; set => this.SetAndRaise(ref field, value); }

   public string Note { get; set => this.SetAndRaise(ref field, value); }

   public int? Validation { get; set => this.SetAndRaise(ref field, value); }

   public int? ValidatorId { get; set => this.SetAndRaise(ref field, value); }

   public string TestName { get; set => this.SetAndRaise(ref field, value); } = "";

   public string Version { get; set => this.SetAndRaise(ref field, value); } = "";


   //public byte[] Code
   //{
   //    get => _code; 
   //    set => this.SetAndRaise(ref _code,value);
   //}
   //private byte[] _code ;
   byte[] IFormTarget.Code => TestClass.Code;

   public string Description { get; set => this.SetAndRaise(ref field, value); } = "";

   public string Specification { get; set => this.SetAndRaise(ref field, value); } = "";

   public string Values { get; set => this.SetAndRaise(ref field, value); } = "";

   string IFormTarget.SpecificationValues { get => Values; set => Values = value; }

   public DateTime? ScheduledDate { get; set => this.SetAndRaise(ref field, value); }

   public DateTime? StartDate { get; set => this.SetAndRaise(ref field, value); }

   public DateTime? EndDate { get; set => this.SetAndRaise(ref field, value); }

   public string OosNo { get; set => this.SetAndRaise(ref field, value); } = "";

   public int? Order { get; set => this.SetAndRaise(ref field, value); }

   public string PharmacopoeiaVersion { get; set => this.SetAndRaise(ref field, value); } = "";

   public int? PharmacopoeiaId { get => _pharmacopoeia.Id; set => _pharmacopoeia.SetId(value); }
   [Ignore] public Pharmacopoeia Pharmacopoeia { get => _pharmacopoeia.Value; set => PharmacopoeiaId = value.Id; }
   readonly ForeignPropertyHelper<SampleTest, Pharmacopoeia> _pharmacopoeia;

   public int? ProductComponentId { get => _productComponent.Id; set => _productComponent.SetId(value); }
   [Ignore] public ProductComponent ProductComponent { get => _productComponent.Value; set => ProductComponentId = value?.Id; }
   readonly ForeignPropertyHelper<SampleTest, ProductComponent> _productComponent;

   [Column("Stage")] public string StageId { get; set => this.SetAndRaise(ref field, value); }

   [Ignore] public SampleTestWorkflow.Stage Stage { get => _stage.Value; set => StageId = value.Name; }
   readonly ObservableAsPropertyHelper<SampleTestWorkflow.Stage> _stage;


   public bool SpecificationDone { get; set => this.SetAndRaise(ref field, value); }

   // RESULT
   public int? ResultId { get => _result.Id; set => _result.SetId(value); }
   [Ignore] public SampleTestResult Result { get => _result.Value; set => ResultId = value.Id; }
   readonly ForeignPropertyHelper<SampleTest, SampleTestResult> _result;

   bool IFormTarget.MandatoryDone
   {
      get => Result?.MandatoryDone ?? true;
      set
      {
         if (Result != null)
            Result.MandatoryDone = value;
      }
   }

   string IFormTarget.Conformity { get => Result?.Conformity;
      set {
         if (Result != null)
            Result.Conformity = value;
      }
   }

   public double Progress { get; set => this.SetAndRaise(ref field, value); }

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

   [Ignore] public int? Color => _color.Value;
   readonly ObservableAsPropertyHelper<int?> _color;

   [Ignore] public string IconPath => _iconPath.Value;

   readonly ObservableAsPropertyHelper<string> _iconPath;


   [Ignore] public ObservableQuery<SampleTestResult> Results;
   // TODO : implement

   //private ObservableQuery<SampleTestResult> _results = H.Property<ObservableQuery<SampleTestResult>>(c => c
   //    .Foreign(e => e.SampleTestId)
   //);


   [Ignore] string IFormTarget.DefaultTestName => TestClass?.Name;

   IFormClass IFormTarget.FormClass { get => TestClass; set => TestClass = (TestClass)value; }
   string IFormTarget.Name { get => TestClass?.Name; set => throw new NotImplementedException(); }

}
