using Microsoft.Win32;
using MupenToolkit.Core.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.UI
{
    public static class WindowsShellWrapper
    {
        public static (string ReturnedPath, bool Cancelled) OpenFileDialogPrompt()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();



            openFileDialog.InitialDirectory = "C:\\";
            openFileDialog.Filter = String.Format("Movie files (*.{0})|*.{0}|All files(*.*)|*.*", InfoProvider.FILE_EXTENSION);
            openFileDialog.Title = "Select a Movie";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;
            var result = openFileDialog.ShowDialog();
            if (result == null || !(bool)result) return (string.Empty, true);
            return (openFileDialog.FileName, false);

        }
        public static (string ReturnedPath, bool Cancelled) SaveFileDialogPrompt()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();


            saveFileDialog.InitialDirectory = "C:\\";
            saveFileDialog.Filter = String.Format("Movie files (*.{0})|*.{0}|All files(*.*)|*.*", InfoProvider.FILE_EXTENSION);
            saveFileDialog.Title = "Select a Movie";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            var result = saveFileDialog.ShowDialog();
            if (result == null || !(bool)result) return (string.Empty, true);
            return (saveFileDialog.FileName, false);

        }
    }
}
