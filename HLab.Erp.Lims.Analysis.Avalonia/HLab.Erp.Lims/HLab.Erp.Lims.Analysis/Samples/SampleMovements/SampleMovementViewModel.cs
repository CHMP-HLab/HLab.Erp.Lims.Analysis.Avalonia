using System.Reactive.Linq;
using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Samples.SampleMovements;

public class SampleMovementViewModel: EntityViewModel<SampleMovement>
{
    internal class Design() : SampleMovementViewModel(null), IDesignViewModel
    {
        public new SampleMovement Model { get; } = SampleMovement.DesignModel;
    }
    public SampleMovementViewModel(Injector i):base(i) {
        _iconPath = this.WhenAnyValue(e => e.Model.Motivation.IconPath)
            .Select((string path) => path??base.IconPath)
            .ToProperty(this, e => e.IconPath)
            ;
    }

    public override string IconPath => _iconPath.Value;
    readonly ObservableAsPropertyHelper<string> _iconPath;
}