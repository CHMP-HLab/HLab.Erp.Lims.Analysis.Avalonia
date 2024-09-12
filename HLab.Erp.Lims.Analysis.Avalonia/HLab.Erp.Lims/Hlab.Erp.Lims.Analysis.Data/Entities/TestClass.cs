using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public partial class TestClass : Entity, ILocalCache, IListableModel, IFormClass
    , IEntityWithColor
{

    public TestClass()
    {
        _caption = this
            .WhenAnyValue(e => e.Name)
            .IfNullOrWhiteSpace("{New Test}")
            //.Select(name => name)
            .ToProperty(this, e => e.Caption)
            ;
        _category = Foreign(this, e => e.CategoryId, e => e.Category);
    }

    public string Name
    {
        get => _name;
        set => SetAndRaise(ref _name, value);
    }

    string _name = "";

    public string IconPath
    {
        get => _iconPath;
        set => SetAndRaise(ref _iconPath, value);
    }

    string _iconPath = "";

    [Ignore] public string Caption => _caption.Value;
    readonly ObservableAsPropertyHelper<string> _caption;

    public string Version
    {
        get => _version;
        set => SetAndRaise(ref _version,value);
    }

    string _version = "";

    public byte[] Code
    {
        get => _code;
        set => SetAndRaise(ref _code,value);
    }

    byte[] _code = Array.Empty<byte>();

    public byte[] Binary
    {
        get => _binary;
        set => SetAndRaise(ref _binary,value);
    }
    byte[] _binary = Array.Empty<byte>() ;

    public int? Order
    {
        get => _order;
        set => SetAndRaise(ref _order,value);
    }

    int? _order ;

    public int? CategoryId
    {
        get => _category.Id;
        set => _category.SetId(value);
    }
    [Ignore]
    public TestCategory Category
    {
        get => _category.Value;
        set => CategoryId = value.Id;
    }
    readonly ForeignPropertyHelper<TestClass,TestCategory> _category;

    [Ignore]
    public virtual ICollection<SampleTest> SampleTests { get; set; }

    public int? DurationFirst
    {
        get => _durationFirst;
        set => SetAndRaise(ref _durationFirst,value);
    }

    int? _durationFirst ;

    public int? DurationNext
    {
        get => _durationNext;
        set => SetAndRaise(ref _durationNext,value);
    }

    int? _durationNext ;

    public int? DurationAdmin
    {
        get => _durationAdmin;
        set => SetAndRaise(ref _durationAdmin,value);
    }

    int? _durationAdmin ;


    //[Column]
    //public int? Color
    //{
    //    get => N.Get(() => (int?)null); set => N.Set(value);
    //}
    [Ignore]
    public int? Color => 0;

    public bool ComponentAware
    {
        get => _componentAware;
        set => SetAndRaise(ref _componentAware,value);
    }
    bool _componentAware ;

    public static TestClass DesignModel => new TestClass
    {
        Name = "Identification",IconPath = "",Version="1.1.0"
    };
}
