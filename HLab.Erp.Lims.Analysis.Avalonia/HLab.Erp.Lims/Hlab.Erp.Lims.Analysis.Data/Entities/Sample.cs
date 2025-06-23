using System;
using System.Reactive.Linq;
using HLab.Base.ReactiveUI;
using HLab.Erp.Acl;
using HLab.Erp.Base.Data;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Data;
using HLab.Erp.Data.foreigners;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data.Entities;


public partial class Sample : Entity, IListableModel
{
    public Sample()
    {

        _validator = this.Foreign( e => e.ValidatorId, e => e.Validator);

        _customer = this.Foreign( e => e.CustomerId, e => e.Customer);

        _manufacturer = this.Foreign( e => e.ManufacturerId, e => e.Manufacturer);

        _pharmacopoeia = this.Foreign( e => e.PharmacopoeiaId, e => e.Pharmacopoeia);

        _product = this.Foreign( e => e.ProductId, e => e.Product);

        _analysisMotivation = this.Foreign( e => e.AnalysisMotivationId, e => e.AnalysisMotivation);

        _expired = this.WhenAnyValue(e => e.ExpirationDate, selector: e => e != null && DateTime.Now > e)
            .ToProperty(this, e => e.Expired);

        _life = this
            .WhenAnyValue(e => e.ExpirationDate, e => e.ManufacturingDate, selector: GetLife)
            .ToProperty(this, e => e.Life);

        _endOfLife = this
            .WhenAnyValue(e => e.ExpirationDate, e => e.Life, selector: GetEndOfLife)
            .ToProperty(this, e => e.EndOfLife);

        _user = this.Foreign( e => e.UserId, e => e.User);


        _stage = this
            .WhenAnyValue(e => e.StageId, id => SampleWorkflow.StageFromName(id))
            .ToProperty(this, e => e.Stage);
    }

    public string FileId { get; set => this.SetAndRaise(ref field, value); }


    public int? UserId { get => _user.Id; set => _user.SetId(value); }
    [Ignore] public User User { get => _user.Value; set => UserId = value.Id; }
    readonly ForeignPropertyHelper<Sample, User> _user;

    public string Reference { get; set => this.SetAndRaise(ref field, value); }

    public string CustomerReference { get; set => this.SetAndRaise(ref field, value); }

    public string ReportReference { get; set => this.SetAndRaise(ref field, value); }

    public DateTime? ReceptionDate { get; set => this.SetAndRaise(ref field, value); }

    public string Worksheet { get; set => this.SetAndRaise(ref field, value); } = "";

    public string CommercialName { get; set => this.SetAndRaise(ref field, value); } = "";

    public string Batch { get; set => this.SetAndRaise(ref field, value); } = "";

    public DateTime? ExpirationDate { get; set => this.SetAndRaise(ref field, value); }

    public bool ExpirationDayValid { get; set => this.SetAndRaise(ref field, value); }

    public DateTime? ManufacturingDate { get; set => this.SetAndRaise(ref field, value); }

    public bool ManufacturingDayValid { get; set => this.SetAndRaise(ref field, value); }

    public DateTime? SamplingDate { get; set => this.SetAndRaise(ref field, value); }

    public bool SamplingDayValid { get; set => this.SetAndRaise(ref field, value); }

    public string SamplingOrigin { get; set => this.SetAndRaise(ref field, value); } = "";

    public string PharmacopoeiaVersion { get; set => this.SetAndRaise(ref field, value); } = "";

    public bool InOriginalPackaging { get; set => this.SetAndRaise(ref field, value); }

    public string PrimaryPackaging { get; set => this.SetAndRaise(ref field, value); } = "";

    public string SecondaryPackaging { get; set => this.SetAndRaise(ref field, value); } = "";

    public double? ReceivedQuantity { get; set => this.SetAndRaise(ref field, value); }

    public double? RemainingQuantity { get; set => this.SetAndRaise(ref field, value); }

    public string Aspect { get; set => this.SetAndRaise(ref field, value); } = "";

    public string Size { get; set => this.SetAndRaise(ref field, value); } = "";

    public bool HasInstruction { get; set => this.SetAndRaise(ref field, value); }

    public bool NoticeFr { get; set => this.SetAndRaise(ref field, value); }

    public bool NoticeEn { get; set => this.SetAndRaise(ref field, value); }

    public string InstructionLanguages { get; set => this.SetAndRaise(ref field, value); } = "";

    public string StorageConditions { get; set => this.SetAndRaise(ref field, value); } = "";

    public string Note { get; set => this.SetAndRaise(ref field, value); } = "";

    public string Conclusion { get; set => this.SetAndRaise(ref field, value); } = "";

    public DateTime? NotificationDate { get; set => this.SetAndRaise(ref field, value); }

    public int? ValidatorId { get => _validator.Id; set => _validator.SetId(value); }

    [Ignore] public User? Validator { get => _validator.Value; set => ValidatorId = value.Id;  }
    readonly ForeignPropertyHelper<Sample, User?> _validator;

    public double Progress { get; set => this.SetAndRaise(ref field, value); }

    public sbyte? Validation { get; set => this.SetAndRaise(ref field, value); }

    public ConformityState ConformityId { get; set => this.SetAndRaise(ref field, value); }

    public string StageId { get; set => this.SetAndRaise(ref field, value); }

    [Ignore] public SampleWorkflow.Stage Stage { get => _stage.Value; set => StageId = value.Name; }

    readonly ObservableAsPropertyHelper<SampleWorkflow.Stage> _stage;
    public string PreviousStageId { get; set => this.SetAndRaise(ref field, value); }


    public int? CustomerId { get => _customer.Id; set => _customer.SetId(value); }
    [Ignore] public Customer? Customer { get => _customer.Value; set => CustomerId = value.Id; }
    readonly ForeignPropertyHelper<Sample, Customer?> _customer;

    public int? ManufacturerId { get => _manufacturer.Id; set => _manufacturer.SetId(value); }
    [Ignore] public virtual Manufacturer Manufacturer { get => _manufacturer.Value; set => ManufacturerId = value.Id; }
    readonly ForeignPropertyHelper<Sample, Manufacturer> _manufacturer;

    public int? PharmacopoeiaId { get => _pharmacopoeia.Id; set => _pharmacopoeia.SetId(value); }
    [Ignore] public Pharmacopoeia Pharmacopoeia { get => _pharmacopoeia.Value; set => PharmacopoeiaId = value.Id; }
    readonly ForeignPropertyHelper<Sample, Pharmacopoeia> _pharmacopoeia;

    public int? ProductId { get => _product.Id;  set => _product.SetId(value); }
    [Ignore] public Product? Product { get => _product.Value; set => ProductId = value.Id; }
    readonly ForeignPropertyHelper<Sample, Product?> _product;

    public int? AnalysisMotivationId { get => _analysisMotivation.Id; set => _analysisMotivation.SetId(value); }
    [Ignore] public AnalysisMotivation AnalysisMotivation { get => _analysisMotivation.Value; set => AnalysisMotivationId = value.Id; }
    readonly ForeignPropertyHelper<Sample, AnalysisMotivation> _analysisMotivation;

    [Ignore] public ObservableQuery<SampleTest> SampleTests;

    // TODO: Implement SampleTests

    //    => _sampleTests.Get();
    //private ObservableQuery<SampleTest> _sampleTests = H.Property<ObservableQuery<SampleTest>>(c => c
    //    .Foreign(e => e.SampleId)
    //);


    [Ignore] public static Sample DesignModel => new()
    {
        Reference = "0042/11/2019",
        ReceivedQuantity = 100,
        ReceptionDate = DateTime.Now,
        Progress = 50,
        User = User.DesignModel

    };

    public bool Invoiced { get; set => this.SetAndRaise(ref field, value); }

    public bool Paid { get; set => this.SetAndRaise(ref field, value); }

    public string InvoiceNo { get; set => this.SetAndRaise(ref field, value); }

    [Ignore] public bool Expired => _expired.Value;
    readonly ObservableAsPropertyHelper<bool> _expired;

    [Ignore] public TimeSpan Life => _life.Value;
    readonly ObservableAsPropertyHelper<TimeSpan> _life;
    static TimeSpan GetLife(DateTime? expirationDate, DateTime? manufacturingDate) => expirationDate == null || manufacturingDate == null ? new TimeSpan(0) : expirationDate.Value - manufacturingDate.Value;

    [Ignore] public bool EndOfLife => _endOfLife.Value;
    readonly ObservableAsPropertyHelper<bool> _endOfLife;

    static bool GetEndOfLife(DateTime? expirationDate, TimeSpan life)
    {
        if (expirationDate == null) return false;
        return DateTime.Now > expirationDate.Value.Subtract(new TimeSpan(life.Ticks / 3));
    }

    [Ignore] public string Caption => Reference;

    [Ignore] public string IconPath => "Icons/Entities/Sample";
}
