using MupenToolkit.Core.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Helper
{
    public static class PathHelper
    {
        public static bool ValidPath(string path)
        {
            bool isValid = true;

            try
            {
                string fullPath = Path.GetFullPath(path);
                if (false)
                    isValid = Path.IsPathRooted(path);
                else
                {
                    string root = Path.GetPathRoot(path);
                    isValid = string.IsNullOrEmpty(root.Trim(new char[] { '\\', '/' })) == false;
                }
                return (InfoProvider.VALID_FILE_EXTENSIONS.Where(ext => Path.GetExtension(path).Contains(ext, StringComparison.InvariantCultureIgnoreCase))).Count() > 0;
                
            }
            catch { isValid = false; }

            return isValid;
        }

    }
}
