﻿using System;
using System.Threading.Tasks;
using HLab.Base.ReactiveUI;
using HLab.Erp.Acl;
using HLab.Erp.Base.Data;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Data;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using HLab.Erp.Lims.Analysis.Products;
using HLab.Mvvm.Annotations;
/* Modification non fusionnée à partir du projet 'HLab.Erp.Lims.Analysis.Module (net6.0-windows)'
Avant :
using HLab.Erp.Lims.Analysis.Data.Entities;
Après :
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab;
using HLab.Erp;
using HLab.Erp.Lims;
using HLab.Erp.Lims.Analysis;
using HLab.Erp.Lims.Analysis.Module;
using HLab.Erp.Lims.Analysis.Module.SampleTests;
using HLab.Erp.Lims.Analysis.Module.Products.ViewModels;
*/

namespace HLab.Erp.Lims.Analysis.Wpf.Products.ViewModels;

 
public class ProductProductComponentsListViewModel : Core.EntityLists.EntityListViewModel<ProductComponent>, IMvvmContextProvider
{
    readonly IDataService _data;
    readonly IAclService _acl;

    public Product Product { get; }
    public ProductProductComponentsListViewModel(IDataService data, IAclService acl, Injector i, Product product) : base(i, c => c
        //.DeleteAllowed()
        .StaticFilter(e => e.ProductId == product.Id)

        .Column("Inn")
        .Header("{Inn}")
        .Width(100)
        .Content(e => e.Inn == null ? "" : e.Inn.Name)
        .OrderBy(e => e.Inn?.Name)
        .OrderByAsc()

        .Column("Dose")
        .Header("{Dose}")
        .Width(100)
        .Content(e => e.Quantity)
        .OrderBy(e => e.Quantity)

        .Column("Unit")
        .Header("{Unit}")
        .Width(100)
        .Content(e => e.Unit.Symbol)
        .OrderBy(e => e.Unit.Symbol)
    )
    {

        Product = product;
        _data = data;
        _acl = acl;

        

        ShowFilters = false;
        ShowMenu = false;
        // List.AddOnCreate(h => h.Entity. = "<Nouveau Critère>").Update();
    }

    public bool EditMode { get => _editMode; set => this.SetAndRaise(ref _editMode,value); }
    bool _editMode = false;

    protected override bool DeleteCanExecute(ProductComponent sampleTest, Action<string> errorAction)
    {
        if (!EditMode) return false;
        if (sampleTest == null) return false;
        //var stage =  sampleTest.Stage.IsAny( errorAction, SampleTestWorkflow.Specifications);
        //var granted = Erp.Acl.IsGranted(errorAction, AnalysisRights.AnalysisAddTest);
        //return stage && granted;
        return true;
    }

    public override Type AddArgumentClass => typeof(Inn);
    public Type AddListClass => typeof(InnsListViewModel);

    // TODO : Add a trigger to refresh the list when the conformity changes
    //readonly ITrigger _1 = H.Trigger(c => c
    //    //.On(e => e.Sample.Stage)
    //    //.On(e => e.Sample.Pharmacopoeia)
    //    //.On(e => e.Sample.PharmacopoeiaVersion)
    //    .On(e => e.EditMode)
    //    .Do(e => (e.AddCommand as CommandPropertyHolder)?.CheckCanExecute())
    //);

    //private readonly ITrigger _triggerConformity = H.Trigger(c => c
    //    .On(e => e.List.Item().Result.ConformityId)
    //    .On(e => e.List.Item().Result.Stage)
    //    .On(e => e.List.Item().Result.Start)
    //    .On(e => e.List.Item().Result.End)
    //    .Do(e => e.List.Refresh())
    //);

    protected override bool AddCanExecute(Action<string> errorAction)
    {
        if (!EditMode) return false;
        if (!_acl.IsGranted(errorAction, AnalysisRights.AnalysisAddTest)) return false;
        //if (Sample.Pharmacopoeia == null)
        //{
        //    errorAction("{Missing} : {Pharmacopoeia}");
        //    return false;
        //}
        //if (string.IsNullOrWhiteSpace(Sample.PharmacopoeiaVersion))
        //{
        //    errorAction("{Missing} : {Pharmacopoeia version}");
        //    return false;
        //}
        //if (! Sample.Stage.IsAny(errorAction,SampleWorkflow.Monograph))
        //{
        //    errorAction("{requier stage} : {Monograph}");
        //    return false;
        //}
        return true;
    }

    protected override async Task ConfigureNewEntityAsync(ProductComponent pc, object arg)
    {
        if (arg is not Inn inn) return;

        var unit = await _data.FetchOneAsync<Unit>(e => e.Symbol=="mg");

        pc.Product = Product;
        pc.Inn = inn;
        pc.Unit = unit;
    }


    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }
}