using HLab.Erp.Lims.Analysis.Data.Entities;

namespace HLab.Erp.Lims.Analysis.Avalonia.Samples
{
   internal class Print
   {
      private string v;
      private string page;
      private string language;

      public Print(string v, string page, string language)
      {
         this.v = v;
         this.page = page;
         this.language = language;
      }

      public string? this[string expirationdate]
      {
         get => throw new System.NotImplementedException();
         set => throw new System.NotImplementedException();
      }

      public object Element { get; set; }

      public void SetData(Sample model)
      {
         throw new System.NotImplementedException();
      }

      public void Cache(string apercu)
      {
         throw new System.NotImplementedException();
      }

      public void AjouteElement()
      {
         throw new System.NotImplementedException();
      }
   }
}