using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Interaction
{
    [NotifyPropertyChanged]
    public class UIError
    {
        public UIError(string title, string message)
        {
            Title = title;
            Message = message;
        }
        public UIError() { }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool Visible { get; set; } = false;


    }

    public static class UIErrorManager
    {

        public static void ShowError(string msg)
        {
            MainWindow.stateContainer.Error.Message = msg;
            MainWindow.stateContainer.Error.Visible = true;
        }
    }
}
