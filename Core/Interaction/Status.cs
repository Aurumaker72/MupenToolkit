using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Interaction
{
    public static class Status
    {
        public enum Sentiment
        {
            Success,
            Fail,
        }
        public enum CurrentMode
        {
            None,
            General,
            ControllerFlagsEditing,

        }
    }
}
