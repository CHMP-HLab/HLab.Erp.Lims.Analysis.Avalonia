﻿using HLab.Base.ReactiveUI;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Data;
using NPoco;

namespace HLab.Erp.Lims.Analysis.Data.Entities;



public class TestClassUnitTest : Entity, IFormTarget
{
    public TestClassUnitTest()
    {
        _testClass = Foreign(this, e => e.TestClassId, e => e.TestClass);
    }

    public int? TestClassId
    {
        get => _testClass.Id;
        set => _testClass.SetId(value);
    }
    [Ignore]
    public TestClass TestClass
    {
        get => _testClass.Value;
        set => TestClassId = value.Id;
    }
    ForeignPropertyHelper<TestClassUnitTest, TestClass> _testClass;
    [Ignore]
    IFormClass IFormTarget.FormClass
    {
        get => TestClass;
        set => TestClass = (TestClass)value;
    }

    [Ignore] byte[] IFormTarget.Code => TestClass?.Code;
    [Ignore] string IFormTarget.DefaultTestName => TestClass?.Name;

    public string Name
    {
        get => _name;
        set => this.SetAndRaise(ref _name, value);
    }

    string _name = "";
    public string ResultValues
    {
        get => _resultValues;
        set => this.SetAndRaise(ref _resultValues, value);
    }

    string _resultValues = "";

    public string SpecificationValues
    {
        get => _specificationValues;
        set => this.SetAndRaise(ref _specificationValues, value);
    }

    string _specificationValues = "";

    public string TestName
    {
        get => _testName;
        set => this.SetAndRaise(ref _testName, value);
    }

    string _testName = "";

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

    public bool SpecificationDone
    {
        get => _specificationDone;
        set => this.SetAndRaise(ref _specificationDone, value);
    }

    bool _specificationDone = false;

    public string Result
    {
        get => _result;
        set => this.SetAndRaise(ref _result, value);
    }

    string _result = "";

    public ConformityState ConformityId
    {
        get => _conformityId;
        set => this.SetAndRaise(ref _conformityId, value);
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }

    ConformityState _conformityId = ConformityState.NotChecked;

    public bool MandatoryDone
    {
        get => _mandatoryDone;
        set => this.SetAndRaise(ref _mandatoryDone, value);
    }

    bool _mandatoryDone = false;

    public string Conformity
    {
        get => _conformity;
        set => this.SetAndRaise(ref _conformity, value);
    }

    string _conformity = "";


    //public string Conform
    //{
    //    get => _conform;
    //    set => this.SetAndRaise(ref _conform,value);
    //}
    //private string _conform = "";

    //public string State
    //{
    //    get => _state;
    //    set => this.SetAndRaise(ref _state,value);
    //}
    //private string _state = "";
}
