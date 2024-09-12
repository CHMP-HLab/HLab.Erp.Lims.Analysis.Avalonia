using HLab.Erp.Lims.Analysis.FormClasses;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Wpf.FormClasses;

public class FormClassViewModelDesign : FormClassViewModel, IDesignViewModel
{
    public FormClassViewModelDesign() : base(null,null,null)
    {
//            Model = FormClass.DesignModel;
        FormHelper.Xaml = "<xml></xml>";
        FormHelper.Cs = "using HLab.Erp.Acl;\nusing HLab.Erp.Lims.Analysis.Data;\nusing HLab.Mvvm.Annotations;";
    }
}