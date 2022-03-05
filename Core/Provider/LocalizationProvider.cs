using Infralution.Localization.Wpf;
using MupenToolkit.Core.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static MupenToolkit.Core.Interaction.Status;

namespace MupenToolkit.Core.Provider
{
    public static class LocalizationProvider
    {
        public static void SetCulture(string CultureString, StateContainer mvm)
        {
            var culture = CultureInfo.GetCultureInfo(CultureString);
            CultureManager.UICulture =
            System.Threading.Thread.CurrentThread.CurrentCulture =
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            mvm.Culture = CultureString;
            Properties.Settings.Default.Culture = CultureString;
            Properties.Settings.Default.Save();
        }
    }
}
