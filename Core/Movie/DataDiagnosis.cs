using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Movie
{
    public static class DataDiagnosis
    {

        public static ObservableCollection<Statistic> GetMovieDiagnosis()
        {
            // TODO: Asynchronousity
            if (MainWindow.stateContainer.Mode != "MovieDiagnosis") return null;
            var diagnosis = new ObservableCollection<Statistic>();

            diagnosis.Add(new Statistic(Properties.Resources.MagicCookieCheck, Properties.Resources.MagicCookieCheckDescription, (MainWindow.stateContainer.Movie.Header.Magic == 0x4D36341A || MainWindow.stateContainer.Movie.Header.Magic == 439629389) ? Properties.Resources.Pass : Properties.Resources.Fail));
            diagnosis.Add(new Statistic(Properties.Resources.VersionCheck, Properties.Resources.VersionCheckDescription, MainWindow.stateContainer.Movie.Header.Version == 3 ? Properties.Resources.Pass : Properties.Resources.Fail));
            diagnosis.Add(new Statistic(Properties.Resources.ControllerBoundsCheck, Properties.Resources.ControllerBoundsCheckDescription, (MainWindow.stateContainer.Movie.Header.Controllers > 0 && MainWindow.stateContainer.Movie.Header.Controllers <= 4) ? Properties.Resources.Pass : Properties.Resources.Fail));
            diagnosis.Add(new Statistic(Properties.Resources.StartupTypeSanityCheck, Properties.Resources.StartupTypeSanityCheckDescription, (MainWindow.stateContainer.Movie.Header.StartFlags == 1 || MainWindow.stateContainer.Movie.Header.StartFlags == 2 || MainWindow.stateContainer.Movie.Header.StartFlags == 4) ? Properties.Resources.Pass : Properties.Resources.Fail));
            diagnosis.Add(new Statistic(Properties.Resources.StandardVISCheck, Properties.Resources.StandardVISCheckDescription, (MainWindow.stateContainer.Movie.Header.VIsPerSecond == 60 || MainWindow.stateContainer.Movie.Header.VIsPerSecond == 30 || MainWindow.stateContainer.Movie.Header.VIsPerSecond == 24) ? Properties.Resources.Pass : Properties.Resources.Fail));
            diagnosis.Add(new Statistic(Properties.Resources.VIBoundsCheck, Properties.Resources.VIBoundsCheckDescription, (MainWindow.stateContainer.Movie.Header.LengthVIs > 0) ? Properties.Resources.Pass : Properties.Resources.Fail));

            return diagnosis;

        }

        public static ObservableCollection<Statistic> GetComboDiagnosis()
        {
            // TODO: Asynchronousity
            if (MainWindow.stateContainer.Mode != "MovieDiagnosis") return null;

            var diagnosis = new ObservableCollection<Statistic>();

            diagnosis.Add(new Statistic("hi", "something", "testing"));

            return diagnosis;

        }

    }
}
