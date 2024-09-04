using System;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Data;
using NPoco;

namespace HLab.Erp.Lims.Analysis.Data;


public class SampleForm : Entity, IFormTarget
{
    public SampleForm() {
        _formClass = Foreign(this, e => e.FormClassId, e => e.FormClass);
        _sample = Foreign(this, e => e.SampleId, e => e.Sample);
    }
    
    public int? FormClassId
    {
        get => _formClass.Id;
        set => _formClass.SetId(value);
    }
    [Ignore]
    public FormClass FormClass
    {
        get => _formClass.Value;
        set => FormClassId = value.Id;
    }
    ForeignPropertyHelper<SampleForm,FormClass> _formClass;

    [Ignore] IFormClass IFormTarget.FormClass 
    {
        get => FormClass;
        set => FormClass = (FormClass)value;
    }


    public int? SampleId
    {
        get => _sample.Id;
        set => _sample.SetId(value);
    }
    [Ignore]
    public Sample Sample
    {
        get => _sample.Value;
        set => SampleId = value.Id;
    }
    ForeignPropertyHelper<SampleForm,Sample> _sample;


    public ConformityState ConformityId
    {
        get =>_conformityId; 
        set =>SetAndRaise(ref _conformityId, value); 
    }
    private ConformityState _conformityId = ConformityState.None;

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public string SpecificationValues
    {
        get => _specificationValues; 
        set => SetAndRaise(ref _specificationValues,value);
    }
    private string _specificationValues ;

    public string ResultValues
    {
        get => _resultValues; 
        set => SetAndRaise(ref _resultValues,value);
    }
    private string _resultValues ;

    public bool MandatoryDone
    {
        get => _mandatoryDone; 
        set => SetAndRaise(ref _mandatoryDone,value);
    }
    private bool _mandatoryDone ;

    public bool SpecificationDone
    {
        get => _specificationDone; 
        set => SetAndRaise(ref _specificationDone,value);
    }
    private bool _specificationDone ;

    byte[] IFormTarget.Code => FormClass.Code;
    string IFormTarget.TestName { get; set; }
    string IFormTarget.Description { get; set; }
    string IFormTarget.Specification { get; set; }
    string IFormTarget.Conformity { get; set; }
    string IFormTarget.Result { get; set; }

    string IFormTarget.DefaultTestName => FormClass.Name;
    string IFormTarget.Name
    {
        get => FormClass.Name;
        set => FormClass.Name = value;
    }
}
