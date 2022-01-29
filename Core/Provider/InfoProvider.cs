using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Provider
{
    public static class InfoProvider
    {
        public static readonly string[] VALID_FILE_EXTENSIONS = new string[] { "m64", "cmb"};
        public enum InteractionTypes { 
        None,
        M64,
        Combo,
        }
        
    }
}
