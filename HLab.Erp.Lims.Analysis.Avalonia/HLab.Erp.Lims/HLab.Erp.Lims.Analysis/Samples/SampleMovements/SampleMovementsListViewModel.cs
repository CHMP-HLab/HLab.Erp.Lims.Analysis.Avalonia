﻿using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Samples.SampleMovements;

public class SampleMovementsListViewModel : EntityListViewModel<SampleMovement>, IMvvmContextProvider
{
    protected override bool AddCanExecute(Action<string> errorAction) => true;
    protected override bool DeleteCanExecute(SampleMovement target,Action<string> errorAction) => Selected!=null || (SelectedIds?.Any()??false);


    readonly Sample? _sample;

    public SampleMovementsListViewModel(Sample? sample, Injector i) : base(i, c => c
        .StaticFilter(e => e.SampleId == sample.Id)
        .HideFilters()

        .ColumnListable(s => s.Motivation)
        .IconPath("Icons/Entities/SampleMovementMotivation")

        .Column("Date")
        .Header("{Date}")
        .Date(e => e.Date)
        .Link(e => e.Date).Filter().IconPath("Icons/Calendar")

        .Column("Quantity")
        .Header("{Quantity}").Content(e => e.Quantity)
    )
    {
        _sample = sample;

        List.Connect()
            .AutoRefreshOnObservable(sm => sm.WhenPropertyChanged(x => x.Quantity))
            .Select(async _ => await UpdateRemainingQuantity())
            .Subscribe();
    }


    readonly SampleTestResult _result;

    public SampleMovementsListViewModel(SampleTestResult result, Injector i) : base(i, c => c
        .StaticFilter(e => e.SampleTestResultId == result.Id)

        .HideFilters()

        .ColumnListable(s => s.Motivation)
        .IconPath("Icons/Entities/SampleMovementMotivation")

        .Column("Date")
        .Header("{Date}")
        .Date(e => e.Date)
        .Link(e => e.Date).Filter().IconPath("Icons/Calendar")

        .Column("Quantity")
        .Header("{Quantity}").Content(e => e.Quantity)
    )
    {
        _sample = result.SampleTest.Sample;
        _result = result;
    }

    public override Type AddArgumentClass => typeof(SampleMovementMotivation);

    protected override Task ConfigureNewEntityAsync(SampleMovement sm, object arg)
    {
        if (arg is not SampleMovementMotivation motivation) return Task.CompletedTask;

        sm.Sample = _sample;
        sm.SampleTestResult = _result;
        sm.Motivation = motivation;
        sm.Quantity = 1;
        sm.Date = DateTime.Today;

        return Task.CompletedTask;
    }

    public async Task UpdateRemainingQuantity()
    {
        if(_sample==null) return;

        try
        {
            var id = _sample.Id;

            //var list = List;
            var list = Injected.Data.FetchWhereAsync<SampleMovement>(m => m.SampleId == id);
            var quantity = _sample.ReceivedQuantity ?? 0;

            await foreach (var movement in list)
            {
                quantity -= movement.Quantity;
            }
            if (_sample.RemainingQuantity.HasValue && Math.Abs(_sample.RemainingQuantity.Value - quantity)<double.Epsilon) return;

            await Injected.Data.UpdateAsync(_sample, s => s.RemainingQuantity = quantity);
        }
        catch(DataException){}
    }

    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }
}