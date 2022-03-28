using MupenToolkitPRE.LowLevel;
using MupenToolkitPRE.UI.Localization;
using MupenToolkitPRE.UI.Platform;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using static MupenToolkitPRE.UI.XAML.DialogHelper;

namespace MupenToolkitPRE.MVVM
{
    public class ChangeThemeCommand : ICommand
    {
        public MainViewModel mvm;
        public bool CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            if (parameter == null) return;
            MainWindow.SetTheme((bool)parameter);
        }
    }
    public class ChangeCultureCommand : ICommand
    {
        public MainViewModel mvm;
        public bool CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            if (parameter == null) return;
            var culture = parameter as string;
            LocalizationHelper.SetCulture(culture, mvm);
            mvm.RefreshPages();
        }
    }
    public static class MovieLoadHelper
    {
        public static void Load(MainViewModel mvm, string path = null)
        {
            string _path = string.Empty;
            if (path == null)
            {
                var shlRet = ShellWrapper.OpenFileDialogPrompt();
                if (shlRet.Cancelled)
                    return;
                _path = shlRet.ReturnedPath;
            }
            else
            {
                _path = path;
            }
            Task.Factory.StartNew(() =>
            {
                var ret = mvm.Movie.Load(_path);
                if (!ret.Status.Success)
                {
                    ShowDialog(ret.Status.FailReason);
                    return;
                }
                mvm.Samples = ret.Samples;
                ShowSnackbar(mvm.MainSnackbar, Properties.Resources.MovieLoadedSuccessfully);
                mvm.FileLoaded = true;
                Properties.Settings.Default.LastPath = _path;
                Properties.Settings.Default.Save(); // save here?
            });
        }
    }
    public class LoadLastCommand : ICommand
    {
        public MainViewModel mvm;
        public bool CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            MovieLoadHelper.Load(mvm, Properties.Settings.Default.LastPath);
        }
    }
    public class LoadCommand : ICommand
    {
        public MainViewModel mvm;
        public bool CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            MovieLoadHelper.Load(mvm);
        }
    }

    public class UnloadCommand : ICommand
    {
        public MainViewModel mvm;
        public bool CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            mvm.FileLoaded = false;
            mvm.Movie = new();
            mvm.Samples = new();
        }
    }
    public class BypassLoadCommand : ICommand
    {
        public MainViewModel mvm;
        public bool CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            ShowSnackbar(mvm.MainSnackbar, Properties.Resources.LoadBypassed);
            mvm.FileLoaded = true;
        }
    }
    public class SeekButtonCommand : ICommand
    {
        public MainViewModel mvm;
        public bool CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            Task.Factory.StartNew(() =>
            {
                if (mvm.CurrentSampleList != null)
                {
                    for (int i = 0; i < mvm.CurrentSampleList.Count; i++)
                    {
                        if (mvm.CurrentSampleList[i].GetButtonByIndex(mvm.StatisticsSelectedButtonIndex))
                        {
                            mvm.StatisticsFoundFrame = i;
                            mvm.StatisticsSearchSuccessful = true;
                            return;
                        }
                    }
                }
                mvm.StatisticsSearchSuccessful = false;
                mvm.StatisticsFoundFrame = -1;
            });
        }
    }
    public class UploadLogCommand : ICommand
    {
        public MainViewModel mvm;
        public bool CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            Directory.CreateDirectory(PathHelper.GetAppLogsFolder());
            var path = Path.Combine(PathHelper.GetAppLogsFolder(), "movie.xml");
            XmlSerializer SerializerObj = new XmlSerializer(typeof(Movie.Definitions.M64.Movie));
            TextWriter WriteFileStream = new StreamWriter(path);
            SerializerObj.Serialize(WriteFileStream, mvm.Movie);
            WriteFileStream.Flush();
            WriteFileStream.Close();
            Process.Start(new ProcessStartInfo()
            {
                FileName = path,
                UseShellExecute = true
            });
        }
    }
    public class SeekJoystickCommand : ICommand
    {
        public MainViewModel mvm;
        public bool CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            Task.Factory.StartNew(() =>
            {
                if (mvm.CurrentSampleList != null)
                {
                    for (int i = 0; i < mvm.CurrentSampleList.Count; i++)
                    {
                        var sample = mvm.CurrentSampleList[i];
                        if (sample.X == mvm.StatisticsSelectedJoystickValue.X && sample.Y == mvm.StatisticsSelectedJoystickValue.Y)
                        {
                            mvm.StatisticsFoundFrame = i;
                            mvm.StatisticsSearchSuccessful = true;
                            return;
                        }
                    }
                }
                mvm.StatisticsSearchSuccessful = false;
                mvm.StatisticsFoundFrame = -1;
            });
        }
    }

    public class SaveCommand : ICommand
    {
        public MainViewModel mvm;
        public bool CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            var shlRet = ShellWrapper.SaveFileDialogPrompt();
            if (shlRet.Cancelled)
                return;
            Task.Factory.StartNew(() =>
            {
                var ret = mvm.Movie.Save(shlRet.ReturnedPath, mvm.Samples);
                if (!ret.Success)
                {
                    ShowDialog(ret.FailReason);
                    return;
                }
                ShowSnackbar(mvm.MainSnackbar, Properties.Resources.MovieSavedSuccessfully);
                mvm.FileLoaded = true;
            });
        }
    }

    public class FrameAdvanceCommand : ICommand
    {
        public MainViewModel mvm;
        public bool CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            if (parameter == null || mvm.CurrentController >= mvm.Samples.Count) return;
            var fInc = Int32.Parse((string)parameter);
            mvm.CurrentSampleIndex = NumericHelper.ClampFrame(mvm.Samples[mvm.CurrentController].Count, mvm.CurrentSampleIndex + fInc);
        }
    }

    public class TogglePlayCommand : ICommand
    {
        public MainViewModel mvm;
        public bool CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            mvm.Movie.Playing ^= true;
        }
    }
    public class CountryChangedCommand : ICommand
    {
        public MainViewModel mvm;
        public bool CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            var ccode = ushort.Parse((string)(parameter));
            mvm.Movie.Header.RomCountry = ccode;
        }
    }
    public class StartFlagsChangedCommand : ICommand
    {
        public MainViewModel mvm;
        public bool CanExecute(object? parameter) => true;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object? parameter)
        {
            var ccode = ushort.Parse((string)(parameter));
            mvm.Movie.Header.StartFlags = ccode;
        }
    }
}
