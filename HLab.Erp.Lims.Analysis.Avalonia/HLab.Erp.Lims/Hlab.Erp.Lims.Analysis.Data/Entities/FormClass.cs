using System;
using System.Reactive.Linq;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public class FormClass : Entity, IListableModel, IEntityWithIcon, IFormClass
{
    public FormClass()
    {
        _caption = this
            .WhenAnyValue(e => e.Name)
            .IfNullOrWhiteSpace("{New form class}")
            .Select(name => $"{{Form class}}\n{name}")
            .ToProperty(this, e => e.Caption);
    }

    public string Name
    {
        get => _name;
        set => SetAndRaise(ref _name, value);
    }
    string _name = "";

    public byte[] Code
    {
        get => _code;
        set => SetAndRaise(ref _code, value);
    }
    byte[] _code = Array.Empty<byte>();

    public string Class
    {
        get => _class;
        set => SetAndRaise(ref _class, value);
    }
    string _class = "";

    public string IconPath
    {
        get => _iconPath;
        set => SetAndRaise(ref _iconPath, value);
    }
    string _iconPath = "";

    public string Version
    {
        get => _version;
        set => SetAndRaise(ref _version, value);
    }
    string _version = "";

    public byte[] CodeHash
    {
        get => _codeHash;
        set => SetAndRaise(ref _codeHash, value);
    }
    byte[] _codeHash = Array.Empty<byte>();

    [Ignore]
    public string Caption => _caption.Value;
    readonly ObservableAsPropertyHelper<string> _caption;

}
