#define GIMO
//#define CHMP

using HLab.Core;
using HLab.Core.Annotations;
using HLab.Erp.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HLab.Erp.Lims.Analysis.Avalonia;

public class ClientConfig(ICryptService crypt, IDataService data) : Bootloader
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
      data.SetConfigureAction(() => Task.FromResult("Host=gimopharm-lims-postgres;Username=lims_001_user;Password=Qv9#eLuR6!xPzA4jWm;Database=gimopharm_lims_001"), true);
#endif

      return base.Load();
   }
}
