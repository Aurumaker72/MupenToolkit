using MupenToolkit.Core.Helper;
using MupenToolkit.Core.Interaction;
using MupenToolkit.Core.Movie;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MupenToolkit.Core.UI
{

    public class LoadMovieCommand : ICommand
    {
        int runs = 0;
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
            if (!PathHelper.ValidPath(shellReturn.ReturnedPath, "m64"))
            {
                mwv.Error.Message = MupenToolkit.Properties.Resources.NotAMovie;
                mwv.Error.Visible ^= true;
                return;
            }

            mwv.Busy = true;

            // reset movie
            mwv.Header = new();
            mwv.Input = new();

            var fs = File.Open(shellReturn.ReturnedPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var headerParsingStatus = MovieManager.ParseHeader(fs);
            if (headerParsingStatus.Status == Core.Interaction.Status.Sentiment.Fail || headerParsingStatus.Header == null)
            {
                mwv.Error.Message = MupenToolkit.Properties.Resources.HeaderParseFailed;
                mwv.Error.Visible ^= true;
                return;
            }
            else
                mwv.Header = headerParsingStatus.Header;

            var stat2 = MovieManager.ParseInputs(fs, headerParsingStatus.Header);
            if (stat2.Status == Core.Interaction.Status.Sentiment.Fail || stat2.Inputs == null)
            {
                mwv.Error.Message = MupenToolkit.Properties.Resources.InputsParseFailed;
                mwv.Error.Visible ^= true;
                return;
            }
            else
                mwv.Input.Samples = stat2.Inputs;


            mwv.Busy = false;

            mwv.Mode = "General";
            mwv.FileLoaded = true;

            mwv.Statistics = InputStatistics.GetStatistics();

        }
    }

    public class UnloadMovieCommand : ICommand
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
            mwv.Header = new();
            mwv.Input = new();

            mwv.FileLoaded = false;
            mwv.Mode = "None";
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
    public class MovieDiagnosisCommand : ICommand
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
                mwv.Diagnosis = MovieDiagnosis.GetDiagnosis();

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
            mwv.Header.RomCountry = ccode;
        }

    }

    public class SaveMovieCommand : ICommand
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
            if (!PathHelper.ValidPath(shellReturn.ReturnedPath, "m64"))
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
            var status = MovieManager.SaveMovie(fs, mwv.Header, mwv.Input.Samples);
            if (status.notifications.Count > 0)
            {
                string final = string.Empty;
                foreach (var item in status.notifications)
                    final += item + "\n";
                MessageBox.Show(final, Properties.Resources.SaveMovie, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK, MessageBoxOptions.None);
            }
            if (status.Sentiment == Status.Sentiment.Fail)
            {
                mwv.Error.Message = status.Error.Message;
                mwv.Error.Visible ^= true;
                mwv.Busy = false;
                return;
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
            //char[] romName = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(mwv.Header.RomName)).ToCharArray();
            //Array.Resize(ref romName, 32);

            //char[] videoPluginName = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(mwv.Header.VideoPluginName)).ToCharArray();
            //Array.Resize(ref videoPluginName, 64);
            //char[] audioPluginName = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(mwv.Header.AudioPluginName)).ToCharArray();
            //Array.Resize(ref audioPluginName, 64);
            //char[] inputPluginName = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(mwv.Header.InputPluginName)).ToCharArray();
            //Array.Resize(ref inputPluginName, 64);
            //char[] rspPluginName = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(mwv.Header.RSPPluginName)).ToCharArray();
            //Array.Resize(ref rspPluginName, 64);
            //char[] author = Encoding.UTF8.GetString(Encoding.ASCII.GetBytes(mwv.Header.Author)).ToCharArray();
            //Array.Resize(ref author, 222);
            //char[] description = Encoding.UTF8.GetString(Encoding.ASCII.GetBytes(mwv.Header.Description)).ToCharArray();
            //Array.Resize(ref description, 256);


            //br.Write(mwv.Header.Magic);
            //br.Write(mwv.Header.Version);
            //br.Write(mwv.Header.UID);
            //br.Write(mwv.Header.LengthVIs);
            //br.Write(mwv.Header.Rerecords);
            //br.Write(mwv.Header.VIsPerSecond);
            //br.Write(mwv.Header.Controllers);
            //br.Seek(sizeof(short), SeekOrigin.Current);
            //br.Write(mwv.Header.LengthSamples);
            //br.Write(mwv.Header.StartFlags);
            //br.Seek(sizeof(short), SeekOrigin.Current);
            //br.Write(mwv.Header.ControllerFlags);
            //br.Seek(sizeof(uint), SeekOrigin.Current); // reservedFlags 
            //br.Seek(48, SeekOrigin.Current);
            //br.Seek(80, SeekOrigin.Current);
            //br.Write(romName);
            //br.Write(mwv.Header.RomCRC);
            //br.Write(mwv.Header.RomCountry);
            //br.Seek(56, SeekOrigin.Current);
            //br.Write(videoPluginName);
            //br.Write(audioPluginName);
            //br.Write(inputPluginName);
            //br.Write(rspPluginName);
            //br.Write(author);
            //br.Write(description);
            if (br != null) br.Flush();
            if (br != null) br.Close();
            if (fs != null) fs.Close();

            mwv.Busy = false;
            mwv.Mode = "General";


        }
    }
}
