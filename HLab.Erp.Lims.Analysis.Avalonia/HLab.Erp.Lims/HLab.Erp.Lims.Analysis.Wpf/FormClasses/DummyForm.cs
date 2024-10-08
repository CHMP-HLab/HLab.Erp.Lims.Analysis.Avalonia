﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Lims.Analysis.FormClasses;
using HLab.Erp.Lims.Analysis.TestClasses;

namespace HLab.Erp.Lims.Analysis.Wpf.FormClasses;

public class DummyForm : UserControl, IForm
{
    public DummyForm():this("Dummy form")
    {
    }

    public DummyForm(string message)
    {
        Content = new TextBlock { Text = message };
    }

    public IFormTarget Target { get; set; }
    public long CreationDuration { get; set; }

    public void Connect(int connectionId, object target){ }

    public void Process(object sender, EventArgs e){ }

    public IFormClassProvider FormClassProvider { get; set; }

    public FormMode Mode { get; set; }

    public string Version => "";

    public IEnumerable<object> NamedElements { get; set; }

    public void SetFormMode(FormMode formMode)
    {
    }

    public void LoadValues(string values)
    {
    }

    public void SetErrorMessage(object fe)
    {
        Content = fe;
    }

    public void Upgrade(FormValues formValues)
    {
    }

    public bool PreventProcess()
    {
        return false;
    }

    public void AllowProcess()
    {
    }

    public void TryProcess(object sender, EventArgs args)
    {
    }
}