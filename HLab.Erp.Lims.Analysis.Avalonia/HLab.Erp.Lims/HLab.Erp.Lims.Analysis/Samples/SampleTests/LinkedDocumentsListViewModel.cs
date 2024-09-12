using HLab.Erp.Core.EntityLists;
using HLab.Erp.Core.ListFilterConfigurators;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Samples.SampleTests;

public class LinkedDocumentsListViewModel(
    EntityListViewModel<LinkedDocument>.Injector i,
    SampleTestResult result)
    : EntityListViewModel<LinkedDocument>(i, c => c
        .HideFilters()
        .StaticFilter(e => e.SampleTestResultId == result.Id)
        .Column("Name")
        .Header("{Name}").Width(200).Content(s => s.Name)), IMvvmContextProvider
{
    //            Expression<Func<LinkedDocument,bool>> filter,
    //            Action<LinkedDocument> createAction = null
    //.DeleteAllowed()

    protected override async Task ConfigureNewEntityAsync(LinkedDocument doc, object arg)
    {
        //TODO Urgent
        //Microsoft.Win32.OpenFileDialog dlg = new()
        //{
        //    DefaultExt = ".pdf",
        //    Filter = "PDF Files (*.pdf)|*.pdf|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"
        //};

        //if (!dlg.ShowDialog() ?? false) throw new DataSetterException("User cancelled");

        //doc.SampleTestResult = result;
        //doc.Name = dlg.FileName.Split('\\').Last();
        //doc.File = await File.ReadAllBytesAsync(dlg.FileName);
    }

    protected override bool DeleteCanExecute(LinkedDocument doc, Action<string> errorAction) => Selected != null;

    public void ConfigureMvvmContext(IMvvmContext ctx)
    {
    }
}