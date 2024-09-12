using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Wpf.Pharmacopoeias;

public class PharmacopoeiaViewModelDesign : PharmacopoeiaViewModel, IDesignViewModel
{
    public PharmacopoeiaViewModelDesign():base(null)
    {
        Model = Pharmacopoeia.DesignModel;
    }
}