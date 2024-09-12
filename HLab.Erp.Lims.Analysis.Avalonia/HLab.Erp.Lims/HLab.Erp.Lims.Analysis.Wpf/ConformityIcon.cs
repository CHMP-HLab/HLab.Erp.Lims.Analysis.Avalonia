using System;
using System.Windows;
using HLab.Base.Wpf.DependencyProperties;
using HLab.Erp.Conformity.Annotations;
using HLab.Icons.Wpf.Icons;

namespace HLab.Erp.Lims.Analysis.Wpf;

using H = DependencyHelper<ConformityIcon>;

public partial class ConformityIcon : IconView
{

    public static readonly DependencyProperty ConformityProperty =
        H.Property<ConformityState>()
            .Default(ConformityState.NotChecked)
            .OnChange(e => e.Update())
            .BindsTwoWayByDefault
            .Register();

    public ConformityState Conformity
    {
        set => SetValue(ConformityProperty, value);
        get => (ConformityState)GetValue(ConformityProperty);
    }

    public static readonly DependencyProperty ShowCaptionProperty =
        H.Property<bool>()
            .Default(false)
            .OnChange(e => e.Update())
            .Register();

    public bool ShowCaption
    {
        set => SetValue(ShowCaptionProperty, value);
        get => (bool)GetValue(ShowCaptionProperty);
    }

    void Update()
    {
        Path = $"Icons/Conformity/{Conformity}";
        if(ShowCaption)
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