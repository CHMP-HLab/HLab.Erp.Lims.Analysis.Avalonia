using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.TestClasses;

public class TestClassViewModelDesign : TestClassViewModel, IDesignViewModel
{
    public TestClassViewModelDesign():base(null,null,null)
    {
        Model = TestClass.DesignModel;
        FormHelper.Xaml = "<xml></xml>";
        FormHelper.Cs = "using HLab.Erp.Acl;\nusing HLab.Erp.Lims.Analysis.Data;\nusing HLab.Mvvm.Annotations;";
    }
}