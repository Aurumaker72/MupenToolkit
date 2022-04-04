using MupenToolkitPRE.Movie.Provider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkitPRE.Movie.Helper
{
    public static class IdentificationHelper
    {
        public static bool ValidMoviePath(this string str)
        {
            return (InfoProvider.ValidFileExtensions.Where(item => Path.GetExtension(str)[1..].Equals(item, StringComparison.InvariantCultureIgnoreCase))).Count() > 0;
        }
    }
}
