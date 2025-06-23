using HLab.Base.ReactiveUI;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Data;
using HLab.Erp.Data.foreigners;
using HLab.Mvvm.Application;
using NPoco;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public class SampleForm : Entity, IFormTarget, IListableModel
{
   public SampleForm()
   {
      _formClass = this.Foreign(e => e.FormClassId, e => e.FormClass);
      _sample = this.Foreign(e => e.SampleId, e => e.Sample);
   }

   public int? FormClassId { get => _formClass.Id; set => _formClass.SetId(value); }
   [Ignore]
   public FormClass FormClass { get => _formClass.Value; set => FormClassId = value.Id; }
   readonly ForeignPropertyHelper<SampleForm, FormClass> _formClass;

   [Ignore]
   IFormClass IFormTarget.FormClass { get => FormClass; set => FormClass = (FormClass)value; }

   public int? SampleId { get => _sample.Id; set => _sample.SetId(value); }
   [Ignore]
   public Sample Sample { get => _sample.Value; set => SampleId = value.Id; }
   readonly ForeignPropertyHelper<SampleForm, Sample> _sample;


   public ConformityState ConformityId { get; set => this.SetAndRaise(ref field, value); } = ConformityState.None;

   public string SpecificationValues { get; set => this.SetAndRaise(ref field, value); } = "";

   public string ResultValues { get; set => this.SetAndRaise(ref field, value); } = "";

   public bool MandatoryDone { get; set => this.SetAndRaise(ref field, value); }

   public bool SpecificationDone { get; set => this.SetAndRaise(ref field, value); }

   byte[] IFormTarget.Code => FormClass.Code;
   string IFormTarget.TestName { get; set; } = "";
   string IFormTarget.Description { get; set; } = "";
   string IFormTarget.Specification { get; set; } = "";
   string IFormTarget.Conformity { get; set; } = "";
   string IFormTarget.Result { get; set; } = "";

   string IFormTarget.DefaultTestName => FormClass.Name;
   string IFormTarget.Name { get => FormClass.Name; set => FormClass.Name = value; }
}
