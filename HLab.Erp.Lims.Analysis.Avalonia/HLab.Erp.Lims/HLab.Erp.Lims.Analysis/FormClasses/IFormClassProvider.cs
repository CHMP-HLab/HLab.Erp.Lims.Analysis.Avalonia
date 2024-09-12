using HLab.Compiler;

namespace HLab.Erp.Lims.Analysis.FormClasses;

public interface IFormClassProvider
{
    IEnumerable<CompileError>? CsErrors { get; }
    IEnumerable<CompileError>? XamlErrors { get; }

    string? ClassName { get; }
    string? BaseClass { get; }

    Dictionary<string, ElementInfo>? NamedElements { get; }

    Task<IForm> CreateAsync();
}