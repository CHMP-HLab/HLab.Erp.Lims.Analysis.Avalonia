using System.Collections.ObjectModel;
using System.Dynamic;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using HLab.Erp.Acl;
using HLab.Erp.Lims.Analysis.Data.Entities;
using HLab.Erp.Lims.Analysis.Data.Workflows;
using Npgsql;
using ReactiveUI;

namespace HLab.Erp.Lims.Analysis.Stats;

public class QueryViewModel : ListableEntityViewModel<StatQuery>
{
    public QueryViewModel(Injector i):base(i)
    {
        //ITrigger OnQuery = H.Trigger(c => c
        //    .On(e => e.Model.Query)
        //    .Do(e => e.SetParamNames())
        //);

        this.WhenAnyValue(e => e.Model.Query)
            .Do(SetParamNames).Subscribe();

        ExportCommand = ReactiveCommand.Create(Export);
        RunCommand = ReactiveCommand.Create(Run);
    }

    public override AclRight EditRight => AnalysisRights.AnalysisStatQueryCreate;

    public ObservableCollection<DataObject> Items {get;} = [];
    public ObservableCollection<string> Columns {get;} = [];


    public string ParameterName1 {get => _parameterName1; set => SetAndRaise(ref _parameterName1,value);}
    string _parameterName1 ;
    public string ParameterName2 {get => _parameterName2; set => SetAndRaise(ref _parameterName2,value);}
    string _parameterName2 ;
    public string ParameterName3 {get => _parameterName3; set => SetAndRaise(ref _parameterName3,value);}
    string _parameterName3 ;
    public string ParameterName4 {get => _parameterName4; set => SetAndRaise(ref _parameterName4,value);}
    string _parameterName4 ;


    public string Parameter1 {get => _parameter1; set => SetAndRaise(ref _parameter1,value);}
    string _parameter1 ;
    public string Parameter2 {get => _parameter2; set => SetAndRaise(ref _parameter2,value);}
    string _parameter2 ;
    public string Parameter3 {get => _parameter3; set => SetAndRaise(ref _parameter3,value);}
    string _parameter3 ;
    public string Parameter4 {get => _parameter4; set => SetAndRaise(ref _parameter4,value);}
    string _parameter4 ;


    void SetParamNames(string query)
    {
        ParameterName1 = GetParamName(query,1);
        ParameterName2 = GetParamName(query,2);
        ParameterName3 = GetParamName(query,3);
        ParameterName4 = GetParamName(query,4);
    }

    public string ErrorMessage {get => _errorMessage; set => SetAndRaise(ref _errorMessage,value);}
    string _errorMessage ;


    public ICommand ExportCommand {get; } 

    void Export()
    {
        // Vérifie qu'il y a bien quelque chose à exporter
        if (Items.Count == 0)
        {
            ErrorMessage = "{Nothing to export}";
            return;
        }

        // TODO URGENT
        /*
         * 
        // Emplacement du fichier
        Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
        dlg.FileName = "Export"; // Default file name
        dlg.DefaultExt = ".csv"; // Default file extension
        dlg.Filter = "Spérateur point-virgule (.csv)|*.csv"; // Filter files by extension
        if (dlg.ShowDialog() != true)
            return;
        
        // Les colonnes
        StringBuilder contenu = new StringBuilder(10000000);
        int nbColonnes = Columns.Count;
        for (int c = 0; c < nbColonnes - 1; c++)
            contenu.Append(CsvValue(Columns[c]) + ";");
        contenu.Append(CsvValue(Columns[nbColonnes - 1]) + "\r\n");

        // Les lignes
        foreach (var ligne in Items)
        {
            for (int c = 1; c < nbColonnes; c++)
                contenu.Append(CsvValue(ligne[c]) + ";");
            contenu.Append(CsvValue(ligne[nbColonnes]) + "\r\n");
        }

        // Enregistre le fichier
        File.WriteAllText(dlg.FileName, contenu.ToString(), Encoding.UTF8);
        */
    }

    string CsvValue(object value)
    {
        if (value == null) return "";
        //if(value is Nullable && ((INullable)value).IsNull) return "";

        if (value is DateTime)
        {
            if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                return ((DateTime)value).ToString("dd/MM/yyyy");
            return ((DateTime)value).ToString("dd/MM/yyyy HH:mm:ss");
        }
        string output = value.ToString();

        if (output.Contains(";") || output.Contains("\""))
            output = '"' + output.Replace("\"", "\"\"") + '"';

        return output;
    }

    string SetParam(string source, int num,string value)
    {
        if(value==null) return source;

        Regex x = new Regex(@$"(/\*{num}:.*?\*)(.*?)(/\*\*/)");
        return x.Replace(source, "$1"+value+"$3");
    }

    string GetParamName(string source, int num)
    {
        Regex x = new Regex(@$"(/\*{num}:)(.*?)(\*/)");
        var match = x.Match(source);
        return match.Groups[2].Value;
    }

    public ICommand RunCommand {get; }

    void Run()
    {
        ErrorMessage = "";

        try
        {
            // Requete
            string requete = Model.Query;// new TextRange(RTB_Requeteur.Document.ContentStart, RTB_Requeteur.Document.ContentEnd).Text;
            requete = SetParam (requete, 1, Parameter1);
            requete = SetParam (requete, 2, Parameter2);
            requete = SetParam (requete, 3, Parameter3);
            requete = SetParam (requete, 4, Parameter4);

            requete = requete.Replace("\r", "").Replace("\n", " ");

            // Execute query
            using var con = new NpgsqlConnection(Injected.Data.ConnectionString);
            Columns.Clear();
            con.Open();
            using var cmd = new NpgsqlCommand(Model.Query, con);
            var reader = cmd.ExecuteReader();
            if (!reader.HasRows)
                throw new Exception("Résultat vide");

            var cols = reader.GetColumnSchema();
            for (int i = 1; i < cols.Count; i++)
            {
                Columns.Add(cols[i].ColumnName);
            }

            while (reader.Read())
            {
                var ligne = new DataObject{
                    Properties=cols.Select(c => c.ColumnName).ToArray(),
                    Values = new string[cols.Count]
                };
                for (int i = 1; i < cols.Count; i++)
                {
                    ligne.Values[i] = reader.GetFieldValue<string>(i);
                }
                Items.Add(ligne);
            }

            //TODO : L_NbResultats.Content = reader.RecordsAffected.ToString();
        }
        catch (Exception ex)
        {
            var e = ex;
            ErrorMessage = "";
            while (e!=null)
            {
                ErrorMessage += ex.Message + Environment.NewLine;
                e = e.InnerException;
            }

            //MessageBox.Show(ex.Message, "ERREUR dans la requete", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}


public class DataObject : DynamicObject
{
    public object[] Values;
    public string[] Properties;

    public object this[string champ]
    {
        get
        {
            // Recherche l'index du champ
            for (int i = 0; i < Properties.Length; i++)
            {
                // Si le nom de la propriété est trouvée, donne la valeur
                if (Properties[i] == champ)
                    return Values[i];
            }

            // Si il ne le trouve pas
            return null;
            //throw new Exception("Ligne : Champ innexistant !");
        }

        set
        {
            // Recherche l'index du champ
            for (int i = 0; i < Properties.Length; i++)
            {
                // Si le nom de la propriété est trouvée, attribue nouvelle la valeur
                if (Properties[i] == champ)
                    Values[i] = value;
            }
        }
    }

    public object this[int index]
    {
        get
        {
            return Values[index];
        }

        set
        {
            Values[index] = value;
        }
    }


    /********************************************************************************************************************************************************************************************************************************************************************************
    * 
    * Demande d'une valeur de proprieté (pour binding par exemple)
    * 
    ***********************************************************************************************************************************************************************************************************************************************************************************/

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        // Recherche l'index du champ
        for (int i = 0; i < Properties.Length; i++)
        {
            // Si le nom de la propriété est trouvée
            if (Properties[i] == binder.Name)
            {
                // Donne la valeur
                result = Values[i];
                return true;
            }
        }

        // La propriété n'a pas été trouvée
        result = null;
        return false;
    }


    /********************************************************************************************************************************************************************************************************************************************************************************
    * 
    * Attribution d'une valeur de proprieté (pour binding par exemple)
    * 
    ***********************************************************************************************************************************************************************************************************************************************************************************/
    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        // Recherche l'index du champ
        for (int i = 0; i < Properties.Length; i++)
        {
            // Si le nom de la propriété est trouvée
            if (Properties[i] == binder.Name)
            {
                // Donne la valeur
                Values[i] = value;
                return true;
            }
        }

        // La propriété n'a pas été trouvée
        return true;
    }

}