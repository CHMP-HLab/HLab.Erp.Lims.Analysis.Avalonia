using HLab.Erp.Data;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;
using System.Reactive.Linq;

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
        set => SetAndRaise(ref _name,value);
    }
    string _name = "";

    public string CasNumber
    {
        get => _casNumber;
        set => SetAndRaise(ref _casNumber,value);
    }

    string _casNumber = "";

    public string UnitGroup
    {
        get => _unitGroup; 
        set => SetAndRaise(ref _unitGroup,value);
    }

    string _unitGroup = "";

    public double? MolarMass
    {
        get => _molarMass; 
        set => SetAndRaise(ref _molarMass,value);
    }

    double? _molarMass = (double?)default;

    public double? Density
    {
        get => _density; 
        set => SetAndRaise(ref _density,value);
    }

    double? _density = (double?)default;

    [Ignore]
    public string Caption => _caption.Value;
    ObservableAsPropertyHelper<string> _caption;


    [Ignore]
    public string IconPath => "IconMolecule";

    public static Inn DesignModel => new (){Name = "Paracetamol"};
}
