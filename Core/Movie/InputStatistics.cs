using MupenToolkit.Core.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Movie
{
    public static class InputStatistics
    {

        public static ObservableCollection<Statistic> GetStatistics()
        {
            // TODO: Asynchronousity
            if (MainWindow.stateContainer.Mode != "InputStatistics") return null;
            if (MainWindow.stateContainer.CurrentController >= MainWindow.stateContainer.Input.Samples.Count || MainWindow.stateContainer.CurrentController < 0) return null;

            var samples = MainWindow.stateContainer.Input.Samples[MainWindow.stateContainer.CurrentController];

            ObservableCollection<Statistic> statistics = new();

            int buttons = 0;
            for (int i = 0; i < samples.Count; i++)
            {
                for (int j = 0; j < 13; j++)
                    if (BitopHelper.GetBit(samples[i].Raw, j)) buttons++;
            }

            statistics.Add(new Statistic(Properties.Resources.ButtonA, String.Format(Properties.Resources.TimesPressedFormat, Properties.Resources.ButtonA), (samples.Where(sample => sample.A)).Count().ToString()));
            statistics.Add(new Statistic(Properties.Resources.ButtonB, String.Format(Properties.Resources.TimesPressedFormat, Properties.Resources.ButtonB), (samples.Where(sample => sample.B)).Count().ToString()));
            statistics.Add(new Statistic(Properties.Resources.TriggerL, String.Format(Properties.Resources.TimesPressedFormat, Properties.Resources.TriggerL), (samples.Where(sample => sample.L)).Count().ToString()));
            statistics.Add(new Statistic(Properties.Resources.TriggerR, String.Format(Properties.Resources.TimesPressedFormat, Properties.Resources.TriggerR), (samples.Where(sample => sample.R)).Count().ToString()));
            statistics.Add(new Statistic(Properties.Resources.TriggerZ, String.Format(Properties.Resources.TimesPressedFormat, Properties.Resources.TriggerZ), (samples.Where(sample => sample.Z)).Count().ToString()));
            statistics.Add(new Statistic(Properties.Resources.CPadUp, String.Format(Properties.Resources.TimesPressedFormat, Properties.Resources.CPadUp), (samples.Where(sample => sample.CPadUp)).Count().ToString()));
            statistics.Add(new Statistic(Properties.Resources.CPadDown, String.Format(Properties.Resources.TimesPressedFormat, Properties.Resources.CPadDown), (samples.Where(sample => sample.CPadDown)).Count().ToString()));
            statistics.Add(new Statistic(Properties.Resources.CPadLeft, String.Format(Properties.Resources.TimesPressedFormat, Properties.Resources.CPadLeft), (samples.Where(sample => sample.CPadLeft)).Count().ToString()));
            statistics.Add(new Statistic(Properties.Resources.CPadRight, String.Format(Properties.Resources.TimesPressedFormat, Properties.Resources.CPadRight), (samples.Where(sample => sample.CPadRight)).Count().ToString()));
            statistics.Add(new Statistic(Properties.Resources.IsABC, Properties.Resources.IsABCDescription, samples.Where(sample => sample.A).Count() == 0 ? Properties.Resources.Yes : Properties.Resources.No));
            statistics.Add(new Statistic(Properties.Resources.TotalButtonPresses, Properties.Resources.TotalButtonPressesDescription, buttons.ToString()));

            return statistics;

        }
    }
}
