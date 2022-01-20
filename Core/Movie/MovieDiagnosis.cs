using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Movie
{
    public static class MovieDiagnosis
    {

        public static ObservableCollection<Statistic> GetDiagnosis()
        {
            // TODO: Asynchronousity
            if (MainWindow.stateContainer.Mode != "MovieDiagnosis") return null;
            var diagnosis = new ObservableCollection<Statistic>();

            diagnosis.Add(new Statistic(Properties.Resources.MagicCookieCheck, Properties.Resources.MagicCookieCheckDescription, (MainWindow.stateContainer.Header.Magic == 0x4D36341A || MainWindow.stateContainer.Header.Magic == 439629389) ? Properties.Resources.Pass : Properties.Resources.Fail));
            diagnosis.Add(new Statistic(Properties.Resources.VersionCheck, Properties.Resources.VersionCheckDescription, MainWindow.stateContainer.Header.Version == 3 ? Properties.Resources.Pass : Properties.Resources.Fail));
            diagnosis.Add(new Statistic(Properties.Resources.ControllerBoundsCheck, Properties.Resources.ControllerBoundsCheckDescription, (MainWindow.stateContainer.Header.Controllers > 0 && MainWindow.stateContainer.Header.Controllers <= 4) ? Properties.Resources.Pass : Properties.Resources.Fail));
            diagnosis.Add(new Statistic(Properties.Resources.StartupTypeSanityCheck, Properties.Resources.StartupTypeSanityCheckDescription, (MainWindow.stateContainer.Header.StartFlags == 1 || MainWindow.stateContainer.Header.StartFlags == 2 || MainWindow.stateContainer.Header.StartFlags == 4) ? Properties.Resources.Pass : Properties.Resources.Fail));
            diagnosis.Add(new Statistic(Properties.Resources.StandardVISCheck, Properties.Resources.StandardVISCheckDescription, (MainWindow.stateContainer.Header.VIsPerSecond == 60 || MainWindow.stateContainer.Header.VIsPerSecond == 30 || MainWindow.stateContainer.Header.VIsPerSecond == 24) ? Properties.Resources.Pass : Properties.Resources.Fail));
            diagnosis.Add(new Statistic(Properties.Resources.VIBoundsCheck, Properties.Resources.VIBoundsCheckDescription, (MainWindow.stateContainer.Header.LengthVIs > 0) ? Properties.Resources.Pass : Properties.Resources.Fail));

            return diagnosis;

        }
    }
}
