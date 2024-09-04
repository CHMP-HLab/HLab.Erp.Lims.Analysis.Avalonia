using System;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data;

public class FormClass : Entity, IListableModel, IEntityWithIcon, IFormClass
{
    public FormClass()
    {
        _caption = this.WhenAnyValue(e => e.Name)
            .ToProperty(this, e => e.Caption);
    }   

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }
    string _name = "";

    public byte[] Code
    {
        get => _code;
        set => this.RaiseAndSetIfChanged(ref _code, value);
    }
    byte[] _code = Array.Empty<byte>();

    public string Class
    {
        get => _class;
        set => this.RaiseAndSetIfChanged(ref _class, value);
    }
    string _class = "";

    public string IconPath
    {
        get => _iconPath;
        set => this.RaiseAndSetIfChanged(ref _iconPath, value);
    }
    string _iconPath = "";

    public string Version
    {
        get => _version;
        set => this.RaiseAndSetIfChanged(ref _version, value);
    }
    string _version = "";

    public byte[] CodeHash
    {
        get => _codeHash;
        set => this.RaiseAndSetIfChanged(ref _codeHash, value);
    }
    byte[] _codeHash = Array.Empty<byte>();

    [Ignore]
    public string Caption => _caption.Value;
    readonly ObservableAsPropertyHelper<string> _caption;

}
