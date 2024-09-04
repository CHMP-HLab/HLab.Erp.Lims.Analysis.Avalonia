using System.Collections.Generic;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;

namespace HLab.Erp.Lims.Analysis.Data;

public partial class TestClass : Entity, ILocalCache, IListableModel, IFormClass
    , IEntityWithIcon
    , IEntityWithColor
{
    public TestClass() { }

    public string Name
    {
        get => _name;
        set => SetAndRaise(ref _name,value);
    }
    private string _name = "";


    public string Version
    {
        get => _version;
        set => SetAndRaise(ref _version,value);
    }
    private string _version = "";


    public byte[] Code
    {
        get => _code;
        set => SetAndRaise(ref _code,value);
    }
    private byte[] _code ;


    public int? Order
    {
        get => _order;
        set => SetAndRaise(ref _order,value);
    }
    private int? _order ;


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
    ForeignPropertyHelper<TestClass,TestCategory> _category;

    [Ignore]
    public virtual ICollection<SampleTest> SampleTests { get; set; }

    public int? DurationFirst
    {
        get => _durationFirst;
        set => SetAndRaise(ref _durationFirst,value);
    }

    private int? _durationFirst ;

    public int? DurationNext
    {
        get => _durationNext;
        set => SetAndRaise(ref _durationNext,value);
    }

    private int? _durationNext ;

    public int? DurationAdmin
    {
        get => _durationAdmin;
        set => SetAndRaise(ref _durationAdmin,value);
    }

    private int? _durationAdmin ;


    //[Column]
    //public int? Color
    //{
    //    get => N.Get(() => (int?)null); set => N.Set(value);
    //}
    [Ignore]
    public int? Color => 0;

    [Ignore]
    public string Caption => _caption.Get();
    private string _caption = H.Property<string>(c => c.Bind(e => e.Name));

    public string IconPath
    {
        get => _iconPath;
        set => SetAndRaise(ref _iconPath,value);
    }

    private string _iconPath ;

    public static TestClass DesignModel => new TestClass
    {
        Name = "Identification",IconPath = "",Version="1.1.0"
    };
}
