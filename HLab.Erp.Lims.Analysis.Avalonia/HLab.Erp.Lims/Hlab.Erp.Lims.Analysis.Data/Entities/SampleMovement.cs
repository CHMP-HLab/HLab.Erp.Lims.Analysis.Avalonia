using System;
using HLab.Base.ReactiveUI;
using HLab.Erp.Data;
using HLab.Erp.Data.foreigners;
using NPoco;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public class SampleMovement : Entity
{
    public SampleMovement()
    {
        _sample = this.Foreign( e => e.SampleId, e => e.Sample);
        _sampleTestResult = this.Foreign( e => e.SampleTestResultId, e => e.SampleTestResult);
        _motivation = this.Foreign(e => e.MotivationId, e => e.Motivation);
    }

    /// <summary>
    /// Sample
    /// </summary>
    public int? SampleId { get => _sample.Id; set => _sample.SetId(value); }
    [Ignore] public virtual Sample? Sample { get => _sample.Value; set => SampleId = value.Id; }
    readonly ForeignPropertyHelper<SampleMovement, Sample?> _sample;

    /// <summary>
    /// SampleTestResult
    /// </summary>
    public int? SampleTestResultId { get => _sampleTestResult.Id; set => _sampleTestResult.SetId(value); }
    [Ignore] public virtual SampleTestResult SampleTestResult { get => _sampleTestResult.Value; set => SampleTestResultId = value?.Id; }
    readonly ForeignPropertyHelper<SampleMovement, SampleTestResult> _sampleTestResult;

    /// <summary>
    /// Motivation
    /// </summary>
    public int? MotivationId { get => _motivation.Id; set => _motivation.SetId(value); }
    [Ignore] public virtual SampleMovementMotivation Motivation{get => _motivation.Value;set => MotivationId = value.Id;}
    readonly ForeignPropertyHelper<SampleMovement, SampleMovementMotivation> _motivation;

    /// <summary>
    /// Quantity
    /// </summary>
    public double Quantity { get; set => this.SetAndRaise(ref field,value);}

    public static SampleMovement DesignModel => new()
    {
        Motivation = new() { Name = "{use}" },
        Quantity = 20,
    };

    public DateTime Date { get => field.ToUniversalTime(); set => field = value.ToUniversalTime(); }
}