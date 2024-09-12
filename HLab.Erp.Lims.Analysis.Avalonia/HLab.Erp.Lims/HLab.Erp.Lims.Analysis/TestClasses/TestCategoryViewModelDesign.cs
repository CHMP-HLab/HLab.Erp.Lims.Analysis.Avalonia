using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Wpf.TestClasses;

public class TestCategoryViewModelDesign : TestCategoryViewModel, IDesignViewModel
{
    public TestCategoryViewModelDesign():base(null)
    {
        Model = TestCategory.DesignModel;
    }
}