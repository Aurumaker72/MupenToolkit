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
        public static CultureInfo[] Cultures = new CultureInfo[]
        {
            CultureInfo.GetCultureInfo("en-US"),
            CultureInfo.GetCultureInfo("de-DE"),
            CultureInfo.GetCultureInfo("pt"),
        };

        public static void SetCulture(CultureInfo Culture, bool OnInit = false)
        {
            if (Cultures.Contains(Culture))
            {
                System.Globalization.CultureInfo cultureInfo = Culture;
                System.Threading.Thread.CurrentThread.CurrentCulture = Culture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = Culture;
                bool found = false;
                for (int i = 0; i < Cultures.Length; i++)
                {
                    if (Cultures[i] == Culture)
                    {
                        Properties.Settings.Default.CultureIndex = i;
                        found = true;
                        break;
                    }
                }
                MainWindow.stateContainer.InternalCulture = Culture.Name;
                

            }
            Properties.Settings.Default.Save();
        }
        public static void SetCulture(string CultureString)
        {
            SetCulture(CultureInfo.GetCultureInfo(CultureString));
        }
        public static Sentiment Initialize()
        {
            try
            {
                SetCulture(Cultures[Properties.Settings.Default.CultureIndex], true);
                MainWindow.stateContainer.InternalCulture = Cultures[Properties.Settings.Default.CultureIndex].Name;
            }
            catch
            {
                return Sentiment.Fail;
            }
            return Sentiment.Success;
        }
    }
}
