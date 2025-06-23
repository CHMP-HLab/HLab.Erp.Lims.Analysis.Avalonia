using System;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using HLab.Core.Annotations;
using HLab.Erp.Lims.Analysis.HtmlXaml;
using HLab.Mvvm.Annotations;

namespace HLab.Erp.Lims.Analysis.Wpf.Prints.HtmlXaml;

internal class HtmlToXamlConverterWpfImpl : IHtmlToXamlConverterPlatformImpl
{
   public class BootLoader : Bootloader
   {
      protected override BootState Load()
      {
         HtmlToXamlConverter.Platform = new HtmlToXamlConverterWpfImpl();
         return base.Load();
      }
   }

   static void SetPropertyValue(XmlElement xamlElement, DependencyProperty property, string stringValue)
   {
         var typeConverter = System.ComponentModel.TypeDescriptor.GetConverter(property.PropertyType);
         try
         {
            var convertedValue = typeConverter.ConvertFromInvariantString(stringValue);
            if (convertedValue != null) xamlElement.SetAttribute(property.Name, stringValue);
         }
         catch (Exception)
         {
            // Handle conversion errors if necessary
         }
   }

   public void SetForegroundPropertyValue(XmlElement xamlElement, string stringValue) => SetPropertyValue(xamlElement, System.Windows.Documents.TextElement.ForegroundProperty, stringValue);

   public void SetBackgroundPropertyValue(XmlElement xamlElement, string stringValue) => SetPropertyValue(xamlElement, System.Windows.Documents.TextElement.BackgroundProperty, stringValue);
}