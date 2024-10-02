using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;
using System.Reactive.Linq;
using HLab.Base.ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public class Inn : Entity, IListableModel, ILocalCache
{
    public Inn()
    {
        _caption = this
            .WhenAnyValue(e => e.Name)
            .IfNullOrWhiteSpace("{New INN}")
            //.Select(name => $"{{INN}}\n{name}")
            .ToProperty(this, e => e.Caption);
    }

    public string Name
    {
        get => _name;
        set => this.SetAndRaise(ref _name,value);
    }
    string _name = "";

    public string CasNumber
    {
        get => _casNumber;
        set => this.SetAndRaise(ref _casNumber,value);
    }

    string _casNumber = "";

    public string UnitGroup
    {
        get => _unitGroup; 
        set => this.SetAndRaise(ref _unitGroup,value);
    }

    string _unitGroup = "";

    public double? MolarMass
    {
        get => _molarMass; 
        set => this.SetAndRaise(ref _molarMass,value);
    }

    double? _molarMass = (double?)default;

    public double? Density
    {
        get => _density; 
        set => this.SetAndRaise(ref _density,value);
    }

    double? _density = (double?)default;

    [Ignore]
    public string Caption => _caption.Value;
    ObservableAsPropertyHelper<string> _caption;


    [Ignore]
    public string IconPath => "IconMolecule";

    public static Inn DesignModel => new (){Name = "Paracetamol"};
}
