using MupenToolkitPRE.LowLevel;
using MupenToolkitPRE.Movie.Definitions;
using MupenToolkitPRE.UI.Localization;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace MupenToolkitPRE.Movie.Manager
{
    public static class StatisticManager
    {

        /// <summary>
        /// TODO: rewrite using Rx
        /// </summary>
        public static void PushStatistics()
        {
            if (MainWindow.MainVM.SelectedPage.Name != Properties.Resources.Statistics) return;
            Task.Factory.StartNew(() =>
            {
                if (MainWindow.MainVM.CurrentController >= MainWindow.MainVM.Samples.Count || MainWindow.MainVM.CurrentController < 0) return;
                var samples = MainWindow.MainVM.Samples[MainWindow.MainVM.CurrentController];
                ObservableCollection<Statistic> statistics = new();
                int buttons = 0;
                for (int i = 0; i < samples.Count; i++)
                {
                    for (int j = 0; j < 13; j++)
                        if (BitOperationsHelper.GetBit(samples[i].Raw, j)) buttons++;
                }
                statistics.Add(new Statistic("A", String.Format(Properties.Resources.TimesPressedFormat, "A"), (samples.Where(sample => sample.A)).Count().ToString()));
                statistics.Add(new Statistic("B", String.Format(Properties.Resources.TimesPressedFormat, "B"), (samples.Where(sample => sample.B)).Count().ToString()));
                statistics.Add(new Statistic("L", String.Format(Properties.Resources.TimesPressedFormat, "L"), (samples.Where(sample => sample.L)).Count().ToString()));
                statistics.Add(new Statistic("R", String.Format(Properties.Resources.TimesPressedFormat, "R"), (samples.Where(sample => sample.R)).Count().ToString()));
                statistics.Add(new Statistic("Z", String.Format(Properties.Resources.TimesPressedFormat, "Z"), (samples.Where(sample => sample.Z)).Count().ToString()));
                statistics.Add(new Statistic(Properties.Resources.ABCCompliant, String.Empty, (samples.Where(sample => sample.A).Count() == 0).ToStringResponse()));

                MainWindow.MainVM.Statistics = statistics;
            });
        }
    }
}
