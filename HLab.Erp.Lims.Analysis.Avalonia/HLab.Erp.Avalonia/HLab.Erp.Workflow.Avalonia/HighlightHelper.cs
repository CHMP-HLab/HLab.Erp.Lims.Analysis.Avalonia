using System;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.VisualTree;
using HLab.Base.Avalonia.Controls;
using HLab.Erp.Workflows.Interfaces;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Workflows;

/// <summary>
/// Décore les champs obligatoires non remplis (cadre rouge + crayon) quand le
/// workflow publie des highlights. Porté de HLab.Erp.Workflows.Wpf : on retrouve
/// le contrôle dont le binding se termine par le nom du champ, puis on pose un
/// MandatoryAdorner dans l'AdornerLayer.
/// </summary>
public static class HighlightHelper
{
    public static void SetHighlights<T>(this IView<T> view, Func<T, IWorkflow?> w)
    {
        if (view is not Control fe) return;

        fe.DataContextChanged += (_, _) =>
        {
            if (fe.DataContext is not T vm) return;

            var workflow = w(vm);

            if (workflow != null)
            {
                Attach(fe, workflow);
            }
            else if (vm is System.ComponentModel.INotifyPropertyChanged npc)
            {
                // Le workflow (OAPH sur Model+Locker) peut ne pas être encore calculé
                // quand le DataContext arrive : on attend qu'il apparaisse.
                System.ComponentModel.PropertyChangedEventHandler? handler = null;
                handler = (_, _) =>
                {
                    var wf = w(vm);
                    if (wf == null) return;
                    npc.PropertyChanged -= handler;
                    Attach(fe, wf);
                };
                npc.PropertyChanged += handler;
            }
        };
    }

    static void Attach(Control fe, IWorkflow workflow)
    {
        // L'état courant a pu être publié avant l'abonnement : le rejouer,
        // et seulement une fois la vue chargée (sinon pas d'AdornerLayer).
        void Replay()
        {
            RemoveHighlights(fe);
            foreach (var s in workflow.Highlights.ToList())
                Highlight(fe, s);
        }

        if (fe.IsLoaded) Replay();
        fe.Loaded += (_, _) => Replay();

        if (workflow.Highlights is INotifyCollectionChanged c)
        {
            c.CollectionChanged += (_, arg) => global::Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                switch (arg.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (arg.NewItems != null)
                            foreach (var item in arg.NewItems)
                            {
                                if (item is string s)
                                    Highlight(fe, s);
                            }
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        RemoveHighlights(fe);
                        break;
                }
            });
        }
    }

    static void Highlight(Control root, string name)
    {
        foreach (var ui in root.GetVisualDescendants().OfType<Control>().ToList())
        {
            var property = BindingProperty(ui);
            if (property is null) continue;

            var expression = BindingOperations.GetBindingExpressionBase(ui, property);
            if (expression is null) continue;

            var path = GetBindingPath(expression);
            if (string.IsNullOrEmpty(path)) continue;

            var bindingName = path.Split('.', ' ', '!', '^').Last();
            if (bindingName != name) continue;

            AddHighlight(ui);
        }
    }

    static void AddHighlight(Control ui)
    {
        var layer = AdornerLayer.GetAdornerLayer(ui);
        if (layer is null) return;

        if (layer.Children.OfType<MandatoryAdorner>()
                .Any(a => ReferenceEquals(AdornerLayer.GetAdornedElement(a), ui)))
            return;

        var adorner = new MandatoryAdorner { IsHitTestVisible = false };
        AdornerLayer.SetAdornedElement(adorner, ui);
        layer.Children.Add(adorner);
    }

    static void RemoveHighlights(Control root)
    {
        var layer = AdornerLayer.GetAdornerLayer(root);
        if (layer is null) return;

        foreach (var adorner in layer.Children.OfType<MandatoryAdorner>().ToList())
            layer.Children.Remove(adorner);
    }

    /// <summary>
    /// Le chemin du binding n'est pas exposé publiquement par Avalonia :
    /// on lit la Description interne de l'expression (ex : "Model.BatchNo").
    /// </summary>
    static string? GetBindingPath(BindingExpressionBase expression)
    {
        var property = expression.GetType().GetProperty(
            "Description",
            System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.NonPublic
            | System.Reflection.BindingFlags.Public);

        return property?.GetValue(expression) as string;
    }

    static AvaloniaProperty? BindingProperty(Control ui) => ui switch
    {
        IMandatoryNotFilled mnf => mnf.MandatoryProperty,
        NumericUpDown => NumericUpDown.ValueProperty,
        CalendarDatePicker => CalendarDatePicker.SelectedDateProperty,
        TextBox => TextBox.TextProperty,
        SelectingItemsControl => SelectingItemsControl.SelectedValueProperty,
        _ => null
    };
}
