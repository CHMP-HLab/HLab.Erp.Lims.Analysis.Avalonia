using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Data.Entities;

namespace HLab.Erp.Lims.Analysis.Wpf.TestClasses;

public class TestCategoryViewModel(EntityViewModel<TestCategory>.Injector i) : ListableEntityViewModel<TestCategory>(i);