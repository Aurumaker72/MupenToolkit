using System;
using System.IO;

namespace MupenToolkitPRE.LowLevel
{
    public static class PathHelper
    {
        public static string GetAppLogsFolder()
        {
            return Path.Combine(Environment.ExpandEnvironmentVariables("%AppData%"), System.AppDomain.CurrentDomain.FriendlyName);
        }
    }
}
