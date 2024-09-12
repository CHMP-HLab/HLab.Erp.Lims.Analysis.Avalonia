using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using HLab.Base.ReactiveUI;
using HLab.Compiler;
using HLab.Erp.Conformity.Annotations;
using HLab.Erp.Lims.Analysis.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace HLab.Erp.Lims.Analysis.FormClasses;

public interface ISampleTestFormClassProvider
{
    Task BuildXamlAsync(string xaml);
    Task<byte[]> BuildCsAsync(string cs);
    Task LoadAsync(byte[] binary);
    Task<IForm> CreateAsync();
    IEnumerable<CompileError> XamlErrors { get; }
    IEnumerable<CompileError> CsErrors { get; }
    IEnumerable<CompileError> DebugErrors { get; }
}

public class FormHelper : ReactiveModel
{
    public async Task LoadDefaultFormAsync(ISampleTestFormClassProvider provider)
    {
        Xaml = "<Grid></Grid>";
        Cs = @"using System;
            using System.Windows;
            using System.Windows.Controls;
            namespace Lims
            {
                public class TestIdentification
                {
                    public void Process(object sender, RoutedEventArgs e)
                    {

                        Test.Description = ""Denomination"";
                        Test.Norme       = ""Specification"";
                        Test.Resultat    = ""Resultat"";
                    }  
                }
            }";
        await CompileAsync(provider).ConfigureAwait(false);
    }

    public IForm Form
    {
        get => _form;
        set => SetAndRaise(ref _form, value);
    }
    IForm _form;

    public ITestResultProvider Result
    {
        get => _result;
        set => SetAndRaise(ref _result, value);
    }
    ITestResultProvider _result;

    public string Xaml
    {
        get => _xaml;
        set { if(SetAndRaise(ref _xaml, value)) FormUpToDate = false; }
    }
    string _xaml;

    public string Cs
    {
        get => _cs;
        set { if (SetAndRaise(ref _cs, value)) FormUpToDate = false; }
    }
    string _cs;

    public byte[] Binary
    {
        get => _binary;
        set => SetAndRaise(ref _binary, value);
    }
    byte[] _binary = [];

    public string XamlMessage
    {
        get => _xamlMessage;
        set => SetAndRaise(ref _xamlMessage, value);
    }
    string _xamlMessage = "";

    public string CsMessage
    {
        get => _csMessage;
        set => SetAndRaise(ref _csMessage, value);
    }
    string _csMessage = "";

    public ObservableCollection<CompileError> XamlErrors { get; } = new();
    public ObservableCollection<CompileError> CsErrors { get; } = new();
    public ObservableCollection<CompileError> DebugErrors { get; } = new();

    public CompileError SelectedXamlError
    {
        get => _selectedXamlError;
        set => SetAndRaise(ref _selectedXamlError,value);
    }

    CompileError _selectedXamlError ;

    public CompileError SelectedCsError
    {
        get => _selectedCsError;
        set => SetAndRaise(ref _selectedCsError,value);
    }

    CompileError _selectedCsError ;

    public CompileError SelectedDebugError
    {
        get => _selectedDebugError;
        set => SetAndRaise(ref _selectedDebugError,value);
    }
    CompileError _selectedDebugError ;

    public bool FormUpToDate
    {
        get => _formUpToDate;
        set => SetAndRaise(ref _formUpToDate,value);
    }
    bool _formUpToDate ;

    public int CsErrorPos
    {
        get => _csErrorPos;
        set => SetAndRaise(ref _csErrorPos,value);
    }
    int _csErrorPos ;

    public ISampleTestFormClassProvider Provider { get; private set; }

    public async Task LoadFormAsync(ISampleTestFormClassProvider provider, IFormTarget target)
    {
        CsMessage = "";
        XamlMessage = "";

        // Loading C#
        if (String.IsNullOrWhiteSpace(Cs))
        {
            CsMessage = "Code was empty";
            return;
        }

        Provider = provider;
        await Provider.BuildXamlAsync(Xaml);

        Binary ??= await Provider.BuildCsAsync(Cs);

        if(Binary!=null)
            await Provider.LoadAsync(Binary);
                
        XamlErrors.Clear();
        CsErrors.Clear();
        DebugErrors.Clear();

        if (Provider.XamlErrors != null)
            foreach (var e in Provider.XamlErrors)
                XamlErrors.Add(e);

        if (Provider.CsErrors != null)
            foreach (var e in Provider.CsErrors)
                CsErrors.Add(e);

        if (Provider.DebugErrors != null)
            foreach (var e in Provider.DebugErrors)
                DebugErrors.Add(e);

        Form = await Provider.CreateAsync();

        Form.Target = target;

        FormUpToDate = true;
    }

    public static int LineCount(string text)
    {
        if (text == null) return 0;
        var size = text.Length;
        var nb = size == 0 ? 0 : 1;
        for (var i = 0; i < size; i++)
            if (text[i] == '\n')
                nb++;
        return nb;
    }

    public async Task ExtractCodeAsync(byte[] code)
    {
        if(code==null) return;

        var sCode = Encoding.UTF8.GetString(await GzipToBytes(code).ConfigureAwait(false));
        var index = sCode.LastIndexOf("}\r\n", StringComparison.InvariantCulture);
        Cs = sCode.Substring(0, index + 1);
        Xaml = sCode.Substring(index + 3);
    }

    public async Task<byte[]> PackCodeAsync()
    {
        var bytes = Encoding.UTF8.GetBytes(Cs.Trim('\r', '\n', ' ') + "\r\n" + Xaml.Trim('\r', '\n', ' '));
        return await BytesToGZip(bytes);
    }

    readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

    // TODO : provider injected here to move things away from wpf.
    public async Task LoadAsync(IFormTarget target, ISampleTestFormClassProvider provider = null)
    {
        await _lock.WaitAsync();
        try
        {
            if (!ReferenceEquals(Form?.Target, target))
            {
                //if (Form?.Target != null) throw new Exception("Target should be null or same");
                //Form.Target = target;
                await ExtractCodeAsync(target.Code).ConfigureAwait(true);

                await LoadFormAsync(provider, target).ConfigureAwait(true);
            }

            Form.PreventProcess();

            if (target?.SpecificationValues != null)
                Form.LoadValues(target.SpecificationValues);

            if (target?.ResultValues != null)
                Form.LoadValues(target.ResultValues);

            Form.AllowProcess();

            Form?.TryProcess(null, EventArgs.Empty);
        }
        finally
        {
            _lock.Release();
        }
    }


    static async Task<byte[]> GzipToBytes(object gz)
    {
        if (gz is byte[] bytes)
        {
            if (bytes.Length == 0)
                return null;

            try
            {
                await using var ms = new MemoryStream();
                await using var bytesSteam = new MemoryStream(bytes);
                await using var gzStream = new GZipStream(bytesSteam, CompressionMode.Decompress);
                await gzStream.CopyToAsync(ms).ConfigureAwait(false);
                return ms.ToArray();
            }
            catch { }
        }
        return null;
    }

    static async Task<byte[]> BytesToGZip(byte[] bytes)
    {
        if (bytes.Length == 0)
            return null;

        try
        {
            await using var ms = new MemoryStream(bytes);
            await using var gz = new MemoryStream();
            await using var zipStream = new GZipStream(gz, CompressionMode.Compress);
            await zipStream.WriteAsync(ms.ToArray(), 0, ms.ToArray().Length);
            zipStream.Close();
            return gz.ToArray();
        }
        catch { }

        return null;
    }
        
    public async Task CompileAsync(ISampleTestFormClassProvider?  provider = null)
    {
        var specs = Form?.Target?.SpecificationValues;
        var values = Form?.Target?.ResultValues;

        await LoadFormAsync(provider, new DummyTarget()).ConfigureAwait(true);

        Form.PreventProcess();

        Form.LoadValues(specs);
        Form.LoadValues(values);

        Form.AllowProcess();

        try
        {
            Form.Process(null, null);
            FormUpToDate = true;
        }
        catch (Exception ex)
        {
            CsMessage += ex.Message;
        }
    }

    const string XamlHeader = @"
        <UserControl 
            xmlns = ""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
            xmlns:x = ""http://schemas.microsoft.com/winfx/2006/xaml""
            xmlns:mc = ""http://schemas.openxmlformats.org/markup-compatibility/2006""
            xmlns:d = ""http://schemas.microsoft.com/expression/blend/2008""
            xmlns:o = ""clr-namespace:HLab.Base.Wpf;assembly=HLab.Base.Wpf""
            xmlns:lang=""clr-namespace:HLab.Localization.Wpf.Lang;assembly=HLab.Localization.Wpf""
            xmlns:math=""clr-namespace:WpfMath.Controls;assembly=WpfMath""
            UseLayoutRounding = ""False"" >

            <UserControl.Resources>
                <ResourceDictionary Source = ""pack://application:,,,/HLab.Erp.Lims.Analysis.Module;component/FormClasses/FormsDictionary.xaml"" />          
            </UserControl.Resources >
            <Grid>
            <Grid.LayoutTransform>
                <ScaleTransform 
                        ScaleX=""{Binding Scale,FallbackValue=4}"" 
                        ScaleY=""{Binding Scale,FallbackValue=4}""/>
                </Grid.LayoutTransform>
        <!--Content-->
            </Grid>
        </UserControl>";
        
    public async Task FormatAsync()
    {
        var tree = CSharpSyntaxTree.ParseText(Cs);
        var root = tree.GetRoot().NormalizeWhitespace();
        Cs = root.ToFullString();

        try
        {
            var doc = XDocument.Parse(XamlHeader.Replace("<!--Content-->",$"{Xaml}"));
            var r = ((XElement)doc.FirstNode).FirstNode.NextNode;
            r = ((XElement)r).FirstNode.NextNode;


            var xaml = ((XElement)r).ToString();

            xaml = xaml.Replace(" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"","");
            xaml = xaml.Replace(" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"","");
            xaml = xaml.Replace(" xmlns:o=\"clr-namespace:HLab.Base.Wpf;assembly=HLab.Base.Wpf\"","");
            xaml = xaml.Replace(" xmlns:lang=\"clr-namespace:HLab.Localization.Wpf.Lang;assembly=HLab.Localization.Wpf\"","");
            xaml = xaml.Replace(" xmlns:math=\"clr-namespace:WpfMath.Controls;assembly=WpfMath\"","");

            Xaml = xaml;
        }
        catch (Exception)
        {
            // Handle and throw if fatal exception here; don't just ignore them
        }
    }

}