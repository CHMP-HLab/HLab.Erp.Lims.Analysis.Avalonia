﻿namespace HLab.Erp.Lims.Analysis.TestClasses;

public class FormValues
{
    Dictionary<string, string> _dict = new();

    public string Version
    {
        get => TryGetValue("Version", out var version) ? version : "";
        set => _dict["Version"] = value;
    }

    public FormValues(string values)
    {
        foreach (var value in values.Split('■'))// Le séparateur est un ALT + 254
        {
            if(!string.IsNullOrEmpty(value))
            {
                var v = value.Split("=");
                if (v.Length > 1)
                {
                    TryAdd(v[0], v[1]);
                }
                else if (v.Length == 1)
                {
                    TryAdd(v[0], "");
                }
            }
        }
    }

    public string Migrate(string oldName,params string[] newNames)
    {
        if(_dict.TryGetValue(oldName, out var value))
        {
            foreach(var newName in newNames) _dict.TryAdd(newName,value);
            _dict.Remove(oldName);
            return value;
        }
        return null;
    }

    public void Migrate(string oldName, string newName, string trueValue)
    {
        if(_dict.TryGetValue(oldName, out var value))
        {
            if(value=="1")
                _dict.TryAdd(newName, trueValue);

            _dict.Remove(oldName);
        }
    }

    public bool TryAdd(string name, string value) => _dict.TryAdd(name, value);
    public bool TryGetValue(string name, out string value) => _dict.TryGetValue(name, out value);
}