using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Lims.Analysis.FormClasses;

namespace HLab.Erp.Lims.Analysis.Wpf.TestClasses;

public abstract class TestLegacyForm : TestForm
{
    protected static class TestEtat
    {
        public static ConformityState NonCommence => ConformityState.NotChecked;
        public static ConformityState EnCours => ConformityState.Running;
        public static ConformityState NonConforme => ConformityState.NotConform;
        public static ConformityState Conforme => ConformityState.Conform;
        public static ConformityState Invalide => ConformityState.Invalid;
    }

    protected TestLegacyForm()
    {
        Test = new LegacyHelper(this);
        
    }

    protected override bool HasLevel(FrameworkElement e,ElementLevel level)
    {
        if (e.Tag is not string tag || string.IsNullOrWhiteSpace(tag)) return base.HasLevel(e, level);
        tag = tag.ToLower();
        return level switch
        {
            ElementLevel.Specification => tag[0] == 'n' || base.HasLevel(e, level),
            ElementLevel.Mandatory => tag[0] == 'o' || base.HasLevel(e, level),
            _ => base.HasLevel(e, level)
        };
    }

    public class LegacyHelper(TestLegacyForm form)
    {
        public ConformityState Etat
        {
            get => form.Target.ConformityId;
            set => form.Target.ConformityId = value;
        }

        public string Conforme
        {
            get => form.Target.Conformity;
            set => form.Target.Conformity = value;
        }

        public string Norme
        {
            get => form.Target.Specification;
            set => form.Target.Specification = value;
        }

        public string NomTest
        {
            get => form.Target.TestName;
            set => form.Target.TestName = value;
        }

        public string Description
        {
            get => form.Target.Description;
            set => form.Target.Description = value;
        }

        public string Resultat
        {
            get => form.Target.Result;
            set => form.Target.Result = value;
        }

        public double Calcul(TextBlock block, double value, int decimals = 2)
        {
            if (double.IsInfinity(value) || double.IsNaN(value))
            {
                block.Background = InvalidBrush;
                block.Text = "!";
            }

            block.Background = ValidBrush;
            block.Text = Math.Round(value, decimals).ToString(CultureInfo.CurrentCulture);
            return value;
        }

        public double EvalueFormule(string formula)
        {
            try
            {
                var engine = new Mages.Core.Engine();
                var result = engine.Interpret(formula);

                if (result != null)
                    return (double)result;
            }
            catch { }

            return double.NaN;
        }

        public void CheckGroupe(object sender, params CheckBox[] group) => CheckGroup(sender, group);

    }
    protected LegacyHelper Test { get; }

}