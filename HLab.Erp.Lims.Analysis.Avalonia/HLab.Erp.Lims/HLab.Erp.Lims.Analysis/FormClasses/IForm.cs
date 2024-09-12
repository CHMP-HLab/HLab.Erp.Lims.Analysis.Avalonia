using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Lims.Analysis.TestClasses;

namespace HLab.Erp.Lims.Analysis.FormClasses;

public interface IForm
{
    IFormTarget Target { get; set; }
    long CreationDuration { get; set; }

    void Connect(int connectionId, object target) { }
    void Process(object sender, EventArgs e) { }

    FormMode Mode { get; set; }
    string Version => string.Empty;

    IEnumerable<object> NamedElements {get; set;}

    void SetFormMode(FormMode formMode);

    void LoadValues(string values);

    bool PreventProcess();
    void AllowProcess();

    void SetErrorMessage(object element);

    public void TryProcess(object sender, EventArgs args);

    void Upgrade(FormValues formValues) { }
}