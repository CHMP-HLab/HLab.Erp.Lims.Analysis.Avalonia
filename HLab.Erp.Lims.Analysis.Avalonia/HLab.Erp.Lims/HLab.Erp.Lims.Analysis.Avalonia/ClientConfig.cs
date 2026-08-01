#define GIMO
//#define CHMP

using HLab.Core;
using HLab.Core.Annotations;
using System.Diagnostics;

namespace HLab.Erp.Lims.Analysis.Avalonia;

/// <summary>
/// Configure les clés de chiffrement du client : la chaîne de connexion vient
/// de la base de registre (Connections\{Source}\Connection, chiffrée) ; si
/// elle est absente, ErpDataBootloader affiche DatabaseConfigView.
/// </summary>
public class ClientConfig(ICryptService crypt) : Bootloader
{
   protected override BootState Load()
   {
#if DEBUG
      var (key, iv) = crypt.GenerateKeys();

      Debug.WriteLine($"Crypt key: {key}");
      Debug.WriteLine($"Crypt iv: {iv}");
#endif
#if CHMP
      crypt.Configure("PG8JaR0ix+GP2w0bXse/ReZugKK+Q/g/", "TuG898IilQA=");
#endif
#if GIMO
      crypt.Configure("h5ju8WPCMQ/T4Q0aIidQdQacXCmbOniH", "+y3URN8fvUg=");
#endif

      return base.Load();
   }
}
