using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Products;

public class FormViewModelDesign() : FormViewModel(null), IDesignViewModel
{
    public new Form Model { get; } = Form.DesignModel;
}