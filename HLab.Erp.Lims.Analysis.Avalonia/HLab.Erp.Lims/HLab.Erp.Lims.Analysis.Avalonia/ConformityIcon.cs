using System;
using Avalonia;
using HLab.Base.Avalonia.DependencyHelpers;
using HLab.Erp.Conformity.Annotations;
using HLab.Icons.Avalonia.Icons;

namespace HLab.Erp.Lims.Analysis.Avalonia;

using H = DependencyHelper<ConformityIcon>;

public class ConformityIcon : IconView
{
    public static readonly StyledProperty<ConformityState> ConformityProperty =
        H.Property<ConformityState>()
            .Default(ConformityState.NotChecked)
            .OnChanged((e, a) => e.Update())
            .BindModeDefault(global::Avalonia.Data.BindingMode.TwoWay)
            .Register();

    public ConformityState Conformity
    {
        set => SetValue(ConformityProperty, value);
        get => GetValue(ConformityProperty);
    }

    public static readonly StyledProperty<bool> ShowCaptionProperty =
        H.Property<bool>()
            .Default(false)
            .OnChanged((e, a) => e.Update())
            .Register();

    public bool ShowCaption
    {
        set => SetValue(ShowCaptionProperty, value);
        get => GetValue(ShowCaptionProperty);
    }

    void Update()
    {
        Path = $"Icons/Conformity/{Conformity}";
        if (ShowCaption)
        {
            Caption = Conformity switch
            {
                ConformityState.NotChecked => "{Not Started}",
                ConformityState.Running => "{Running}",
                ConformityState.NotConform => "{Not Conform}",
                ConformityState.Conform => "{Conform}",
                ConformityState.Invalid => "{Not Valid}",
                ConformityState.None => "{Unknown}",
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
