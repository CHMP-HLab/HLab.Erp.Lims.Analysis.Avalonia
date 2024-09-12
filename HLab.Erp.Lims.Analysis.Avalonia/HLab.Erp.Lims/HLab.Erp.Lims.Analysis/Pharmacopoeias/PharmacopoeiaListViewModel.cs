using System;
using HLab.Erp.Core;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Wpf.Pharmacopoeias;

public class PharmacopoeiasListViewModel(EntityListViewModel<Pharmacopoeia>.Injector i)
    : Core.EntityLists.EntityListViewModel<Pharmacopoeia>(i, c => c
        .Column("Name")
        .Header("{Name}")
        .Width(250)
        .Localize(e => e.Name)
        .OrderByAsc(0)
        .Icon(p => p.IconPath)
        .Link(e => e.Name)
        .Filter()
        .Column("Abbreviation")
        .Header("{Abbreviation}")
        .Width(250)
        .Link(e => e.Abbreviation)
        .Filter()), IMvvmContextProvider
{
    public class Bootloader : ParamBootloader { }

    protected override bool AddCanExecute(Action<string> errorAction) => true;
    protected override bool DeleteCanExecute(Pharmacopoeia pharmacopoeia, Action<string> errorAction) => true;

    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }

}