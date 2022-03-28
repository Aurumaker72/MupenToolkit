using Infralution.Localization.Wpf;
using MupenToolkitPRE.MVVM;
using System.Globalization;

namespace MupenToolkitPRE.UI.Localization
{
    public static class LocalizationHelper
    {
        public static void SetCulture(string CultureString, MainViewModel mvm)
        {
            CultureInfo culture;
            try
            {
                culture = CultureInfo.GetCultureInfo(CultureString);
            }
            catch
            {
                culture = CultureInfo.CurrentCulture;
            }
            CultureManager.UICulture =
            System.Threading.Thread.CurrentThread.CurrentCulture =
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            Properties.Settings.Default.Culture = CultureString;
            Properties.Settings.Default.Save();
        }
    }
}
