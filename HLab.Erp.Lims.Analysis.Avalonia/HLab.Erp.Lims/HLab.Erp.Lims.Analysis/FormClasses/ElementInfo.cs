namespace HLab.Erp.Lims.Analysis.FormClasses;

public readonly struct ElementInfo(string name, Type type, ElementLevel level)
{
    public string Name { get; } = name;
    public Type Type { get; } = type;
    public ElementLevel Level { get; } = level;
}