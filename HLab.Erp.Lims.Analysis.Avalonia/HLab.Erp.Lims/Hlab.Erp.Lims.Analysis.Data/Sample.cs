using System;
using System.Reactive.Linq;
using HLab.Erp.Acl;
using HLab.Erp.Base.Data;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Data;
using HLab.Erp.Data.Observables;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Mvvm.Application;
using NPoco;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Data;


public partial class Sample : Entity, IListableModel
{
    public Sample() {

        _validator = Foreign(this, e => e.ValidatorId, e => e.Validator);

        _customer = Foreign(this, e => e.CustomerId, e => e.Customer);

        _manufacturer = Foreign(this, e => e.ManufacturerId, e => e.Manufacturer);

        _pharmacopoeia = Foreign(this, e => e.PharmacopoeiaId, e => e.Pharmacopoeia);

        _product = Foreign(this, e => e.ProductId, e => e.Product);

        _analysisMotivation = Foreign(this, e => e.AnalysisMotivationId, e => e.AnalysisMotivation);

        _expired = this.WhenAnyValue(e => e.ExpirationDate, selector: e => e != null && DateTime.Now > e)
            .ToProperty(this, e => e.Expired);

        _life = this
            .WhenAnyValue(e => e.ExpirationDate, e => e.ManufacturingDate, selector: GetLife)
            .ToProperty(this, e => e.Life);

        _endOfLife = this
            .WhenAnyValue(e => e.ExpirationDate, e => e.Life, selector: GetEndOfLife)
            .ToProperty(this, e => e.EndOfLife);

        _user = Foreign(this, e => e.UserId, e => e.User);


        _stage = this
            .WhenAnyValue(e => e.StageId, id => SampleWorkflow.StageFromName(id))
            .ToProperty(this, e => e.Stage);


    }

    public string FileId
    {
        get => _fileId;
        set => SetAndRaise(ref _fileId,value);
    }
    private string _fileId ;


    public int? UserId
    {
        get => _user.Id; 
        set => _user.SetId(value);
    }
    [Ignore]
    public User User
    {
        get => _user.Value; 
        set => UserId = value.Id;
    }
    ForeignPropertyHelper<Sample, User> _user;


    public string Reference
    {
        get => _reference;
        set => SetAndRaise(ref _reference,value);
    }
    private string _reference ;


    public string CustomerReference
    {
        get => _customerReference;
        set => SetAndRaise(ref _customerReference,value);
    }
    private string _customerReference ;


    public string ReportReference
    {
        get => _reportReference;
        set => SetAndRaise(ref _reportReference,value);
    }
    private string _reportReference ;


    public DateTime? ReceptionDate
    {
        get => _receptionDate;
        set => SetAndRaise(ref _receptionDate,value);
    }
    private DateTime? _receptionDate ;


    public string Worksheet

    {
        get => _worksheet;
        set => SetAndRaise(ref _worksheet,value);
    }
    private string _worksheet = "";


    public string CommercialName
    {
        get => _commercialName;
        set => SetAndRaise(ref _commercialName,value);
    }
    private string _commercialName = "";


    public string Batch
    {
        get => _batch;
        set => SetAndRaise(ref _batch,value);
    }
    private string _batch = "";


    public DateTime? ExpirationDate
    {
        get => _expirationDate;
        set => SetAndRaise(ref _expirationDate,value);
    }
    private DateTime? _expirationDate ;


    public bool ExpirationDayValid
    {
        get => _expirationDayValid;
        set => SetAndRaise(ref _expirationDayValid,value);
    }
    private bool _expirationDayValid ;


    public DateTime? ManufacturingDate
    {
        get => _manufacturingDate;
        set => SetAndRaise(ref _manufacturingDate,value);
    }
    private DateTime? _manufacturingDate ;


    public bool ManufacturingDayValid
    {
        get => _manufacturingDayValid;
        set => SetAndRaise(ref _manufacturingDayValid,value);
    }
    private bool _manufacturingDayValid ;
    
    
    public DateTime? SamplingDate
    {
        get => _samplingDate;
        set => SetAndRaise(ref _samplingDate,value);
    }
    private DateTime? _samplingDate ;


    public bool SamplingDayValid
    {
        get => _samplingDayValid;
        set => SetAndRaise(ref _samplingDayValid,value);
    }
    private bool _samplingDayValid ;


    public string SamplingOrigin
    {
        get => _samplingOrigin;
        set => SetAndRaise(ref _samplingOrigin,value);
    }
    private string _samplingOrigin = "";


    public string PharmacopoeiaVersion
    {
        get => _pharmacopoeiaVersion;
        set => SetAndRaise(ref _pharmacopoeiaVersion,value);
    }
    private string _pharmacopoeiaVersion = "";


    public bool InOriginalPackaging
    {
        get => _inOriginalPackaging;
        set => SetAndRaise(ref _inOriginalPackaging,value);
    }
    private bool _inOriginalPackaging ;


    public string PrimaryPackaging
    {
        get => _primaryPackaging;
        set => SetAndRaise(ref _primaryPackaging,value);
    }
    private string _primaryPackaging = "";


    public string SecondaryPackaging
    {
        get => _secondaryPackaging;
        set => SetAndRaise(ref _secondaryPackaging,value);
    }
    private string _secondaryPackaging = "";


    public double? ReceivedQuantity
    {
        get => _receivedQuantity;
        set => SetAndRaise(ref _receivedQuantity,value);
    }
    private double? _receivedQuantity ;


    public string Aspect
    {
        get => _aspect;
        set => SetAndRaise(ref _aspect,value);
    }
    private string _aspect = "";


    public string Size
    {
        get => _size;
        set => SetAndRaise(ref _size,value);
    }
    private string _size = "";


    public bool HasInstruction
    {
        get => _hasInstruction;
        set => SetAndRaise(ref _hasInstruction,value);
    }
    private bool _hasInstruction ;


    public bool NoticeFr
    {
        get => _noticeFr;
        set => SetAndRaise(ref _noticeFr,value);
    }
    private bool _noticeFr ;


    public bool NoticeEn
    {
        get => _noticeEn;
        set => SetAndRaise(ref _noticeEn,value);
    }
    private bool _noticeEn ;


    public string InstructionLanguages
    {
        get => _instructionLanguages;
        set => SetAndRaise(ref _instructionLanguages,value);
    }
    private string _instructionLanguages = "";


    public string StorageConditions
    {
        get => _storageConditions;
        set => SetAndRaise(ref _storageConditions,value);
    }
    private string _storageConditions = "";


    public string Note
    {
        get => _note;
        set => SetAndRaise(ref _note,value);
    }
    private string _note = "";


    public string Conclusion
    {
        get => _conclusion;
        set => SetAndRaise(ref _conclusion,value);
    }
    private string _conclusion = "";


     public DateTime? NotificationDate
    {
        get => _notificationDate;
        set => SetAndRaise(ref _notificationDate,value);
    }
    private DateTime? _notificationDate ;


    public int? ValidatorId
    {
        get => _validator.Id;
        set => _validator.SetId(value);
    }


    [Ignore]
    public User Validator
    {
        get => _validator.Value;
        set => ValidatorId = value.Id;
    }
    ForeignPropertyHelper<Sample, User> _validator;

    public double Progress
    {
        get => _progress;
        set => SetAndRaise(ref _progress,value);
    }
    private double _progress ;


    public sbyte? Validation
    {
        get => _validation;
        set => SetAndRaise(ref _validation,value);
    }
    private sbyte? _validation ;


    public ConformityState ConformityId
    {
        get => _conformityId;
        set => SetAndRaise(ref _conformityId,value);
    }
    private ConformityState _conformityId ;

    [Column("Stage")]
    public string StageId
    {
        get => _stageId;
        set => SetAndRaise(ref _stageId,value);
    }
    private string _stageId ;

    [Ignore]
    public SampleWorkflow.Stage Stage
    {
        get => _stage.Value;
        set => StageId = value.Name;
    }
    ObservableAsPropertyHelper<SampleWorkflow.Stage> _stage;


    public int? CustomerId
    {
        get => _customer.Id;
        set => _customer.SetId(value);
    }
    [Ignore]
    public Customer Customer
    {
        get => _customer.Value;
        set => CustomerId = value.Id;
    }
    ForeignPropertyHelper<Sample, Customer> _customer;

    public int? ManufacturerId
    {
        get => _manufacturer.Id;
        set => _manufacturer.SetId(value);
    }
    [Ignore]
    public virtual Manufacturer Manufacturer
    {
        get => _manufacturer.Value;
        set => ManufacturerId = value.Id;
    }
    ForeignPropertyHelper<Sample, Manufacturer> _manufacturer;


    public int? PharmacopoeiaId
    {
        get => _pharmacopoeia.Id;
        set => _pharmacopoeia.SetId(value);
    }
    [Ignore]
    public Pharmacopoeia Pharmacopoeia
    {
        get => _pharmacopoeia.Value;
        set => PharmacopoeiaId = value.Id;
    }
    ForeignPropertyHelper<Sample, Pharmacopoeia> _pharmacopoeia;


    public int? ProductId
    {
        get => _product.Id;
        set => _product.SetId(value);
    }
    [Ignore]
    public Product Product
    {
        get => _product.Value;
        set => ProductId = value.Id;
    }
    ForeignPropertyHelper<Sample, Product> _product;

    public int? AnalysisMotivationId
    {
        get => _analysisMotivation.Id;
        set => _analysisMotivation.SetId(value);
    }
    [Ignore]
    public AnalysisMotivation AnalysisMotivation
    {
        get => _analysisMotivation.Value;
        set => AnalysisMotivationId = value.Id;
    }
    ForeignPropertyHelper<Sample, AnalysisMotivation> _analysisMotivation;

    [Ignore] public ObservableQuery<SampleTest> SampleTests => _sampleTests.Get();
    private ObservableQuery<SampleTest> _sampleTests = H.Property<ObservableQuery<SampleTest>>(c => c
        .Foreign(e => e.SampleId)
    );

    
    [Ignore]
    public static Sample DesignModel => new Sample
    {
            Reference = "0042/11/2019",
            ReceivedQuantity = 100,
            ReceptionDate = DateTime.Now,
            Progress = 50,
            User = User.DesignModel

    };

    public bool Invoiced
    {
        get => _invoiced;
        set => SetAndRaise(ref _invoiced,value);
    }
    private bool _invoiced ;


    public bool Paid
    {
        get => _paid;
        set => SetAndRaise(ref _paid,value);
    }
    private bool _paid ;


    public string InvoiceNo
    {
        get => _invoiceNo;
        set => SetAndRaise(ref _invoiceNo,value);
    }
    private string _invoiceNo ;

    [Ignore]
    public bool Expired => _expired.Value;
    ObservableAsPropertyHelper<bool> _expired; 
    

    [Ignore]
    public TimeSpan Life => _life.Value;
    readonly ObservableAsPropertyHelper<TimeSpan> _life;
    TimeSpan GetLife(DateTime? expirationDate, DateTime? manufacturingDate) => expirationDate == null || manufacturingDate == null ? new TimeSpan(0) : expirationDate.Value - manufacturingDate.Value;

    [Ignore]
    public bool EndOfLife => _endOfLife.Value;
    readonly ObservableAsPropertyHelper<bool> _endOfLife;

    static bool GetEndOfLife(DateTime? expirationDate, TimeSpan life)
    {
        if (expirationDate == null) return false;
        return DateTime.Now > expirationDate.Value.Subtract(new TimeSpan(life.Ticks / 3));
    }

    [Ignore]
    public string Caption => Reference;

    [Ignore]
    public string IconPath => "Icons/Entities/Sample";
}
