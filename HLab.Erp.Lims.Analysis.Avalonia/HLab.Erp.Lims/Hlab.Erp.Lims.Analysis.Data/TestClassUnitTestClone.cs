﻿using System.Collections.Generic;
using System.Linq;
using HLab.Base.ReactiveUI;
using HLab.Erp.Conformity.Annotations;
using NPoco;

namespace HLab.Erp.Lims.Analysis.Data;
public static class TestClassUnitTestExtension
{
    public static bool Check(this IFormTarget reference, IFormTarget test, out string error)
    {
        var spec = CompareValues(reference.SpecificationValues, test.SpecificationValues);
        var value = CompareValues(reference.ResultValues, test.ResultValues);

        var result = spec.Concat(value).ToList();

        if (result.Count == 0)
        {
            error = "";
            return true;
        }
        error = string.Join("\r\n",result);
        return false;
    }

        class Entry
    {
        public string Name;
        public string Reference;
        public string Value;

        public override string ToString() => $@"{Name} = {Reference} <> {Value}";
    }

        static IEnumerable<Entry> CompareValues(string v1, string v2)
    {
        var a1 = v1.Split("■").ToHashSet();
        var a2 = v2.Split("■").ToHashSet();

        foreach (var v in a1.ToList())
        {
            if (a2.Contains(v))
            {
                a1.Remove(v);
                a2.Remove(v);
            }
        }

        var d = new Dictionary<string, Entry>();
        foreach (var v in a1)
        {
            var k = v.Split("=");
            if (k.Length == 2)
            {
                if (d.ContainsKey(k[0]))
                {
                    d[k[0]].Name = k[0];
                    d[k[0]].Reference = k[1];
                }
                else d.Add(k[0],new Entry{Reference = k[1]});
            }
        }
        foreach (var v in a2)
        {
            var k = v.Split("=");
            if (k.Length == 2)
            {
                if (d.ContainsKey(k[0]))
                {
                    d[k[0]].Name = k[0];
                    d[k[0]].Value = k[1];
                }
                else d.Add(k[0],new Entry{Value = k[1]});
            }
        }

        return d.Values;
    }

    public static void Load(this IFormTarget target, IFormTarget source)
    {
        target.FormClass = source.FormClass;
        target.Name = source.Name;

        target.SpecificationValues = source.SpecificationValues;
        target.ResultValues = source.ResultValues;

        target.SpecificationDone = source.SpecificationDone;
        target.MandatoryDone = source.MandatoryDone;

        target.TestName = source.TestName;
        target.Description = source.Description;
        target.Specification = source.Specification;
        target.Result = source.Result;
        target.Conformity = source.Conformity;
        target.ConformityId = source.ConformityId;
    }

    public static T Clone<T>(this IFormTarget source) where T : IFormTarget, new()
    {
        return new T
        {
            FormClass = source.FormClass,
            Name = source.Name,
            SpecificationValues = source.SpecificationValues,
            ResultValues = source.ResultValues,
            SpecificationDone = source.SpecificationDone,
            MandatoryDone = source.MandatoryDone,
            TestName = source.TestName,
            Description = source.Description,
            Specification = source.Specification,
            Result = source.Result,
            Conformity = source.Conformity,
            ConformityId = source.ConformityId
        };
    }
}

public class TestClassUnitTestClone : ReactiveModel, IFormTarget
{
    public TestClassUnitTestClone() { }

    public IFormClass FormClass
    {
        get => _formClass;
        set => SetAndRaise(ref _formClass,value);
    }
    private IFormClass _formClass ;


    [Ignore] byte[] IFormTarget.Code => FormClass?.Code;
    [Ignore] string IFormTarget.DefaultTestName => FormClass?.Name;

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
