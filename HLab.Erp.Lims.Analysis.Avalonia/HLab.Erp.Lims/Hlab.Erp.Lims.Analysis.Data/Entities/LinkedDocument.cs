using HLab.Erp.Data;
using NPoco;
using System;
using System.Diagnostics;
using System.IO;

namespace HLab.Erp.Lims.Analysis.Data.Entities;

public class LinkedDocument : Entity
{
    public LinkedDocument()
    {
        _sampleTestResult = Foreign(this, e => e.SampleTestResultId, e => e.SampleTestResult);
    }

    public void OpenDocument()
    {
        var path = Path.GetTempFileName() + "_" + Name;

        System.IO.File.WriteAllBytes(path, File);

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true
        };
        Process.Start(psi);
    }

    public string Name
    {
        get => _name;
        set => SetAndRaise(ref _name, value);
    }
    string _name = "";

    public int? SampleTestResultId
    {
        get => _sampleTestResult.Id;
        set => _sampleTestResult.SetId(value);
    }
    [Ignore]
    public SampleTestResult SampleTestResult
    {
        get => _sampleTestResult.Value;
        set => SampleTestResultId = value.Id;
    }
    readonly ForeignPropertyHelper<LinkedDocument, SampleTestResult> _sampleTestResult;

    public byte[] File
    {
        get => _file;
        set => SetAndRaise(ref _file, value);
    }
    byte[] _file = Array.Empty<byte>();
}
