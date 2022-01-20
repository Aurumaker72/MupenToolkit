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
        public static bool ValidPath(string path, string searchExt)
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
                if (!Path.GetExtension(path).Contains(searchExt, StringComparison.InvariantCultureIgnoreCase)) return false;
            }
            catch { isValid = false; }

            return isValid;
        }

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
            }
            catch { isValid = false; }

            return isValid;
        }
    }
}
