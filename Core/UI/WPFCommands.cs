using MupenToolkit.Core.Helper;
using MupenToolkit.Core.Interaction;
using MupenToolkit.Core.Movie;
using MupenToolkit.Core.Provider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MupenToolkit.Core.UI
{
    


    public class LoadDataCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            var shellReturn = WindowsShellWrapper.OpenFileDialogPrompt();
            if (shellReturn.Cancelled) return;
            if (!PathHelper.ValidPath(shellReturn.ReturnedPath))
            {
                mwv.Error.Message = MupenToolkit.Properties.Resources.NotAMovie;
                mwv.Error.Visible ^= true;
                return;
            }
            MovieManager.AttemptLoad(mwv, shellReturn.ReturnedPath);

           

        }
    }
    
    public class LoadLastCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            MovieManager.AttemptLoad(mwv, Properties.Settings.Default.MovieLastPath);
        }
    }
    // UNUSED
    public class JoystickClickCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object? parameter)
        {
            return true;
        }
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object? parameter)
        {
            var canvas = parameter as Canvas;
            Point p = Mouse.GetPosition(canvas);

            mwv.SelectedSample.X = (sbyte)MathHelper.Clamp(Math.Round(p.X), -127, 127);
            mwv.SelectedSample.Y = (sbyte)MathHelper.Clamp(Math.Round(p.Y), -127, 127);
        }
    }

    public class UnloadDataCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            mwv.Movie.Header = new();
            mwv.Input = new();

            mwv.FileLoaded = false;
            mwv.Mode = "None";
            mwv.InteractionMode = Provider.InfoProvider.InteractionTypes.None;
        }
    }



    public class EditControllerFlagsCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            if (!mwv.FileLoaded) return; // ???
            mwv.Mode = mwv.Mode == "ControllerFlagsEditing" ? "General" : "ControllerFlagsEditing";
        }
    }

    public class EditCountryCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            if (!mwv.FileLoaded) return; // ???
            mwv.Mode = mwv.Mode == "CountryEditing" ? "General" : "CountryEditing";
        }
    }
    
    public class InputStatisticsCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            if (!mwv.FileLoaded) return; // ???
            mwv.Mode = mwv.Mode == "InputStatistics" ? "General" : "InputStatistics";
            if (mwv.Mode == "InputStatistics")
                mwv.Statistics = InputStatistics.GetStatistics();

        }
    }

    public class BypassMovieCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            mwv.Mode = "General";
            mwv.FileLoaded = true;
        }
    }

    

    public class DataDiagnosisCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            if (!mwv.FileLoaded) return; // ???
            mwv.Mode = mwv.Mode == "MovieDiagnosis" ? "General" : "MovieDiagnosis";
            if (mwv.Mode == "MovieDiagnosis")
                mwv.Diagnosis = mwv.InteractionMode == Provider.InfoProvider.InteractionTypes.M64 ? DataDiagnosis.GetMovieDiagnosis() : DataDiagnosis.GetComboDiagnosis();

        }
    }
    public class LanguagePickerCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            if (!mwv.FileLoaded) return; // ???
            mwv.Mode = mwv.Mode == "LanguagePicker" ? "General" : "LanguagePicker";

        }
    }

    public class SetLanguageCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            if (parameter == null) return;
            var lstr = parameter as string;
            mwv.Busy = true;
            LocalizationProvider.SetCulture(lstr);
            mwv.Busy = false;
        }
    }

    public class ShowAboutCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            MessageBox.Show(Properties.Resources.AboutText);
        }
    }

    public class CountryChangedCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            var ccode = (ushort.Parse((string)(parameter)));
            mwv.Movie.Header.RomCountry = ccode;
        }

    }

    public class SampleIndexChangeCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            if (parameter == null) return;
            var i = int.Parse((string)parameter);
            //if (mwv.CurrentSampleIndex + i < 0 || mwv.CurrentSampleIndex > mwv.Header.LengthSamples) return;
            // i = increment
            if (mwv.CurrentController > mwv.Input.Samples.Count || mwv.CurrentController < 0) return;
            if (mwv.CurrentSampleIndex + i < 0 || mwv.CurrentSampleIndex > mwv.Input.Samples[mwv.CurrentController].Count) return;
            mwv.CurrentSampleIndex += i;
        }

    }
    public class SaveDataCommand : ICommand
    {
        public StateContainer mwv;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            var shellReturn = WindowsShellWrapper.SaveFileDialogPrompt();
            if (shellReturn.Cancelled) return;
            if (!PathHelper.ValidPath(shellReturn.ReturnedPath))
            {
                mwv.Error.Message = MupenToolkit.Properties.Resources.NotAMovie;
                mwv.Error.Visible ^= true;
                return;
            }

            mwv.Busy = true;

            FileStream fs = null;
            BinaryWriter br = null;

#if !DEBUG
            try
            {
#endif
            fs = File.Open(shellReturn.ReturnedPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            br = new BinaryWriter(fs);
                if (mwv.InteractionMode == Provider.InfoProvider.InteractionTypes.M64)
                {
                    var status = MovieManager.SaveMovie(fs, mwv.Movie.Header, mwv.Input.Samples);
                    if (status.notifications.Count > 0)
                    {
                        string final = string.Empty;
                        foreach (var item in status.notifications)
                            final += item + "\n";
                        MessageBox.Show(final, Properties.Resources.SaveData, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.None);
                    }
                    if (status.Sentiment == Status.Sentiment.Fail)
                    {
                        mwv.Error.Message = status.Error.Message;
                        mwv.Error.Visible ^= true;
                        mwv.Busy = false;
                        return;
                    }
                }
                else if (mwv.InteractionMode == Provider.InfoProvider.InteractionTypes.Combo)
                {
                    var status = MovieManager.SaveCombo(fs, mwv.Combos, mwv.Input.Samples);
                    if (status.Sentiment == Status.Sentiment.Fail)
                    {
                        mwv.Error.Message = status.Error.Message;
                        mwv.Error.Visible ^= true;
                        mwv.Busy = false;
                        return;
                    }
                }
                else
                {
                    throw new NotImplementedException("Unimplemented mode");
                }
#if !DEBUG
        }
            catch
            {
                mwv.Error.Message = MupenToolkit.Properties.Resources.SaveFailed;
                mwv.Error.Visible ^= true;
                mwv.Busy = false;
            }
#endif
            if (br != null) br.Flush();
            if (br != null) br.Close();
            if (fs != null) fs.Close();

            mwv.Busy = false;
            mwv.Mode = "General";


        }
    }
}
