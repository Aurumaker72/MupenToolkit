using Microsoft.Win32;
using MupenToolkitPRE.Movie.Provider;
using System;

namespace MupenToolkitPRE.UI.Platform
{
    public static class ShellWrapper
    {
        public static (string ReturnedPath, bool Cancelled) OpenFileDialogPrompt()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "C:\\";
            string filter = "Movie files |";
            for (int i = 0; i < InfoProvider.ValidFileExtensions.Length; i++)
            {
                string? item = InfoProvider.ValidFileExtensions[i];
                if (i == InfoProvider.ValidFileExtensions.Length - 1) filter += ($"*.{item}");
                else filter += String.Format("*.{0};", item);
            }
            openFileDialog.Filter = filter;
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
            string filter = "Movie files |";
            for (int i = 0; i < InfoProvider.ValidFileExtensions.Length; i++)
            {
                string? item = InfoProvider.ValidFileExtensions[i];
                if (i == InfoProvider.ValidFileExtensions.Length - 1) filter += ($"*.{item}");
                else filter += String.Format("*.{0};", item);
            }
            saveFileDialog.Filter = filter;
            saveFileDialog.Title = "Select a Movie";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.RestoreDirectory = true;
            var result = saveFileDialog.ShowDialog();
            if (result == null || !(bool)result) return (string.Empty, true);
            return (saveFileDialog.FileName, false);

        }
    }
}
