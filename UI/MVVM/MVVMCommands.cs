using MupenToolkitPRE.LowLevel;
using MupenToolkitPRE.Movie.Helper;
using MupenToolkitPRE.UI.Localization;
using MupenToolkitPRE.UI.Platform;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using static MupenToolkitPRE.UI.XAML.DialogHelper;
using System.Linq;

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
            if (mvm.Busy)
            {
                ShowDialogIf(Properties.Resources.AlreadyBusy, Properties.Resources.Info, Properties.Settings.Default.NotifyOnRaceConditionAvoidance);
                return;
            }

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
            if (!_path.ValidMoviePath()) return;
            Task.Factory.StartNew(() =>
            {
                mvm.Busy = true;
                var ret = mvm.Movie.Load(_path);
                if (!ret.Status.Success)
                {
                    mvm.Busy = false;
                    ShowDialog(ret.Status.FailReason, Properties.Resources.MovieLoadFailed);
                    return;
                }
                mvm.Samples = ret.Samples;
                ShowSnackbar(mvm.MainSnackbar, Properties.Resources.MovieLoadedSuccessfully);
                mvm.FileLoaded = true;
                mvm.Busy = false;
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
            if (mvm.Busy)
            {
                ShowDialogIf(Properties.Resources.AlreadyBusy, Properties.Resources.Info, Properties.Settings.Default.NotifyOnRaceConditionAvoidance);
                return;
            }
            Task.Factory.StartNew(() =>
            {
                mvm.Busy = true;
                if (mvm.CurrentSampleList != null)
                {
                    for (int i = 0; i < mvm.CurrentSampleList.Count; i++)
                    {
                        if (mvm.CurrentSampleList[i].GetButtonByIndex(mvm.StatisticsSelectedButtonIndex))
                        {
                            mvm.StatisticsFoundFrame = i;
                            mvm.StatisticsSearchSuccessful = true;
                            mvm.Busy = false;
                            return;
                        }
                    }
                }
                mvm.Busy = false;
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
            try
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
            catch(InvalidOperationException opEx)
            {
                ShowDialog(Properties.Resources.SerializationInvalidOperation, Properties.Resources.GenericError);
            }
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
            if (mvm.Busy)
            {
                ShowDialogIf(Properties.Resources.AlreadyBusy, Properties.Resources.Info, Properties.Settings.Default.NotifyOnRaceConditionAvoidance);
                return;
            }
            Task.Factory.StartNew(() =>
            {
                mvm.Busy = true;
                if (mvm.CurrentSampleList != null)
                {
                    for (int i = 0; i < mvm.CurrentSampleList.Count; i++)
                    {
                        var sample = mvm.CurrentSampleList[i];
                        if (sample.X == mvm.StatisticsSelectedJoystickValue.X && sample.Y == mvm.StatisticsSelectedJoystickValue.Y)
                        {
                            mvm.StatisticsFoundFrame = i;
                            mvm.StatisticsSearchSuccessful = true;
                            mvm.Busy = false;
                            return;
                        }
                    }
                }
                mvm.Busy = false;
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
            if (mvm.Busy)
            {
                ShowDialogIf(Properties.Resources.AlreadyBusy, Properties.Resources.Info, Properties.Settings.Default.NotifyOnRaceConditionAvoidance);
                return;
            }
            var shlRet = ShellWrapper.SaveFileDialogPrompt();
            if (shlRet.Cancelled)
                return;
            Task.Factory.StartNew(() =>
            {
                mvm.Busy = true;
                var ret = mvm.Movie.Save(shlRet.ReturnedPath, mvm.Samples);
                if (!ret.Success)
                {
                    mvm.Busy = false;
                    ShowDialog(ret.FailReason, Properties.Resources.MovieSaveFailed);
                    return;
                }
                ShowSnackbar(mvm.MainSnackbar, Properties.Resources.MovieSavedSuccessfully);
                mvm.Busy = false;
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
    public class FrameSetCommand : ICommand
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
            mvm.CurrentSampleIndex = NumericHelper.ClampFrame(mvm.Samples[mvm.CurrentController].Count, (int)parameter);
            // navigate to analog input page :/ MVVM violation
            if (Properties.Settings.Default.AutomaticFocusNavigation)
            {
                foreach (var item in mvm.PageItems.Where(item => item._ContentType == typeof(AnalogInputPage)))
                {
                    mvm.SelectedPage = item;
                    break;
                }
            }
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
