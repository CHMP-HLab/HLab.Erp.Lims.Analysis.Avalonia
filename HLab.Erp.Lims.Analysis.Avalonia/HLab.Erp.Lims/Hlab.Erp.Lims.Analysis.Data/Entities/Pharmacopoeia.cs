using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using HLab.Base.ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public static class ObservablesExtensions
{
    public static IObservable<string> IfNullOrWhiteSpace(this IObservable<string> obs, string value)
        => obs.Select(s => string.IsNullOrWhiteSpace(s) ? value : s);
}

public partial class Pharmacopoeia : Entity, IListableModel, ILocalCache
{
    public Pharmacopoeia() => _caption = this
        .WhenAnyValue(e => e.Name)
        .IfNullOrWhiteSpace("{New pharmacopoeia}")
        .ToProperty(this, e => e.Caption);

    public string Name
    {
        get => _name;
        set => this.SetAndRaise(ref _name, value);
    }

    string _name = "";

    public string Abbreviation
    {
        get => _abbreviation;
        set => this.SetAndRaise(ref _abbreviation, value);
    }

    string _abbreviation = "";
    //public string LastVersion
    //{
    //    get => _lastVersion;
    //    set => this.SetAndRaise(ref _lastVersion,value);
    //}
    //private string _lastVersion = "";


    public string Url
    {
        get => _url;
        set => this.SetAndRaise(ref _url, value);
    }

    string _url = "";


    public string SearchUrl
    {
        get => _searchUrl;
        set => this.SetAndRaise(ref _searchUrl, value);
    }

    string _searchUrl = "";


    public string ReferenceUrl
    {
        get => _referenceUrl;
        set => this.SetAndRaise(ref _referenceUrl, value);
    }

    string _referenceUrl = "";

    public string IconPath
    {
        get => _iconPath;
        set => this.SetAndRaise(ref _iconPath, value);
    }

    string _iconPath = "";

    [Ignore]
    public string Caption => _caption.Value;
    ObservableAsPropertyHelper<string> _caption;


    public static Pharmacopoeia DesignModel => new Pharmacopoeia
    {
        Name = "{US Pharmacopoeia}",
        Abbreviation = "Usp"
    };

}

