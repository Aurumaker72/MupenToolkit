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
            if (shellReturn.Cancelled || !PathHelper.ValidPath(shellReturn.ReturnedPath, "m64"))
            {
                mwv.Error.Message = MupenToolkit.Properties.Resources.NotAMovie;
                mwv.Error.Visible = true;
                return;
            }

            // reset movie
            mwv.Header = new();
            mwv.Input = new();

            var fs = File.Open(shellReturn.ReturnedPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var headerParsingStatus = MovieManager.ParseHeader(fs);
            if (headerParsingStatus.Status == Core.Interaction.Status.Sentiment.Fail || headerParsingStatus.Header == null)
            {
                mwv.Error.Message = MupenToolkit.Properties.Resources.HeaderParseFailed;
                mwv.Error.Visible = true;
                return;
            }
            else
                mwv.Header = headerParsingStatus.Header;

            var stat2 = MovieManager.ParseInputs(fs, headerParsingStatus.Header);
            if (stat2.Status == Core.Interaction.Status.Sentiment.Fail || stat2.Inputs == null)
            {
                mwv.Error.Message = MupenToolkit.Properties.Resources.InputsParseFailed;
                mwv.Error.Visible = true;
                return;
            }
            else
                mwv.Input.Samples = stat2.Inputs;


            mwv.Error.Visible = false;


            mwv.FileLoaded = true;
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
        }
    }

    public class TASStudioEditCommand : ICommand
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
            if (shellReturn.Cancelled || !PathHelper.ValidPath(shellReturn.ReturnedPath, "m64"))
            {
                mwv.Error.Message = MupenToolkit.Properties.Resources.NotAMovie;
                mwv.Error.Visible = true;
                return;
            }

            FileStream fs   = null; 
            BinaryWriter br = null;

            try
            {
                fs = File.Open(shellReturn.ReturnedPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                br = new BinaryWriter(fs);


                // this is reused from Mupen Utilities
                byte[] zeroar1 = new byte[160]; byte[] zeroar2 = new byte[56];
                byte[] magic = new byte[4] { 0x4D, 0x36, 0x34, 0x1A };
                Array.Clear(zeroar1, 0, zeroar1.Length);
                Array.Clear(zeroar2, 0, zeroar2.Length);

                br.Write(mwv.Header.Magic); // Int32 - Magic (4D36341A)
                br.Write(mwv.Header.Version); // UInt32 - Version number (3)
                br.Write(mwv.Header.UID); // UInt32 - UID
                br.Write((UInt32)mwv.Header.LengthVIs); // UInt32 - VIs
                br.Write((UInt32)mwv.Header.Rerecords); // UInt32 - RRs
                br.Write(mwv.Header.VIsPerSecond); // Byte - VI/s
                br.Write(mwv.Header.Controllers); // Byte - Controllers
                br.Write((Int16)0); // 2 Bytes - RESERVED
                br.Write(mwv.Header.LengthSamples); // UInt32 - Input Samples

                br.Write((UInt16)mwv.Header.StartFlags); // UInt16 - Movie start type
                br.Write((Int16)0); // 2 bytes - RESERVED
                br.Write(mwv.Header.ControllerFlags); // UInt32 - Controller Flags
                br.Write(zeroar1, 0, zeroar1.Length); // 160 bytes - RESERVED
                byte[] romname = new byte[32];
                romname = ASCIIEncoding.ASCII.GetBytes(mwv.Header.RomName);
                Array.Resize(ref romname, 32);
                br.Write(romname, 0, 32);
                br.Write(mwv.Header.RomCRC);
                br.Write(mwv.Header.RomCountry);
                br.Write(zeroar2, 0, zeroar2.Length); // 56 bytes - RESERVED


                byte[] gfx = new byte[64];
                byte[] audio = new byte[64];
                byte[] input = new byte[64];
                byte[] rsp = new byte[64];
                byte[] author = new byte[222];
                byte[] desc = new byte[256];

                gfx = Encoding.ASCII.GetBytes(mwv.Header.VideoPluginName);
                audio = Encoding.ASCII.GetBytes(mwv.Header.AudioPluginName);
                input = Encoding.ASCII.GetBytes(mwv.Header.InputPluginName);
                rsp = Encoding.ASCII.GetBytes(mwv.Header.RSPPluginName);
                author = Encoding.UTF8.GetBytes(mwv.Header.Author);
                desc = Encoding.UTF8.GetBytes(mwv.Header.Description);

                Array.Resize(ref gfx, 64);
                Array.Resize(ref audio, 64);
                Array.Resize(ref input, 64);
                Array.Resize(ref rsp, 64);
                Array.Resize(ref author, 222);
                Array.Resize(ref desc, 256);

                br.Write(gfx, 0, 64);
                br.Write(audio, 0, 64);
                br.Write(input, 0, 64);
                br.Write(rsp, 0, 64);
                br.Write(author, 0, 222);
                br.Write(desc, 0, 256);

                if (br.BaseStream.Position != 1024)
                {
                    mwv.Error.Message = MupenToolkit.Properties.Resources.SaveAlignment;
                    mwv.Error.Visible = true;
                }

                br.BaseStream.Seek(1024, SeekOrigin.Begin);

                for (int i = 0; i < mwv.Input.Samples[0].Count; i++)
                {
                    if (BitopHelper.GetBit(mwv.Header.ControllerFlags, 0))
                        br.Write(mwv.Input.Samples[0][i].Raw);
                    if (BitopHelper.GetBit(mwv.Header.ControllerFlags, 1) && i < mwv.Input.Samples[1].Count)
                        br.Write(mwv.Input.Samples[1][i].Raw);
                    if (BitopHelper.GetBit(mwv.Header.ControllerFlags, 2) && i < mwv.Input.Samples[2].Count)
                        br.Write(mwv.Input.Samples[2][i].Raw);
                    if (BitopHelper.GetBit(mwv.Header.ControllerFlags, 3) && i < mwv.Input.Samples[3].Count)
                        br.Write(mwv.Input.Samples[3][i].Raw);
                }

            }
            catch
            {
                mwv.Error.Message = MupenToolkit.Properties.Resources.SaveFailed;
                mwv.Error.Visible = true;
            }

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
            if(br!=null)br.Flush();
            if(br!=null)br.Close();
            if(fs != null)fs.Close();

            mwv.Error.Visible = false;

            

        }
    }
}
