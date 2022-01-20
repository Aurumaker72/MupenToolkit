using MupenToolkit.Core.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static MupenToolkit.Core.Interaction.Status;

namespace MupenToolkit.Core.Movie
{
    public static class MovieManager
    {
        private static BinaryReader br;

        public static (MovieHeader? Header, Sentiment Status) ParseHeader(FileStream fs)
        {
            br = new BinaryReader(fs);
            br.BaseStream.Seek(0, SeekOrigin.Begin);

            MovieHeader header = new();

            try
            {
                header.Magic = br.ReadUInt32();
                header.Version = br.ReadUInt32();
                header.UID = br.ReadUInt32();
                header.LengthVIs = br.ReadUInt32();
                header.Rerecords = br.ReadUInt32();
                header.VIsPerSecond = br.ReadByte();
                header.Controllers = br.ReadByte();
                br.ReadBytes(2);
                header.LengthSamples = br.ReadUInt32();
                header.StartFlags = br.ReadUInt16();
                br.ReadBytes(2);
                header.ControllerFlags = new ControllersBitflag(br.ReadUInt32());
                br.ReadBytes(160);
                header.RomName = Encoding.ASCII.GetString(br.ReadBytes(32));
                header.RomCRC = br.ReadUInt32();
                header.RomCountry = br.ReadUInt16();
                br.ReadBytes(56);

                header.VideoPluginName = Encoding.ASCII.GetString(br.ReadBytes(64));
                header.AudioPluginName = Encoding.ASCII.GetString(br.ReadBytes(64));
                header.InputPluginName = Encoding.ASCII.GetString(br.ReadBytes(64));
                header.RSPPluginName = Encoding.ASCII.GetString(br.ReadBytes(64));

                header.Author = Encoding.ASCII.GetString(br.ReadBytes(222));
                header.Description = Encoding.ASCII.GetString(br.ReadBytes(256));

                return (header, Sentiment.Success);
            }
            catch
            {
                return (null,Sentiment.Fail);
            }
        }

        public static (ObservableCollection<ObservableCollection<Sample>>? Inputs, Sentiment Status) ParseInputs(FileStream fs, MovieHeader header, bool closeFileStream = true)
        {
            
            br.BaseStream.Seek(1024, SeekOrigin.Begin);

            ObservableCollection<ObservableCollection<Sample>> inputs = new();

            var frames = header.LengthSamples;
            var curFrame = 0;
            var lenFs = fs.Length;
            bool[] ctlEnabled = new bool[4];
            for (int i = 0; i < 4; i++)
            {
                ctlEnabled[i] = BitopHelper.GetBit(header.ControllerFlags.Raw, i);
                if (ctlEnabled[i])
                {
                    inputs.Add(new ObservableCollection<Sample>());
                }
            }
            while (curFrame < frames)
            {
                if (ctlEnabled[0])
                {
                    if (br.BaseStream.Position + sizeof(int) > lenFs || 0 < 0 || 0 >= inputs.Count)
                        break;
                    inputs[0].Add(new Sample(br.ReadInt32()));
                }
                if (ctlEnabled[1])
                {
                    if (br.BaseStream.Position + sizeof(int) > lenFs || 1 < 0 || 1 >= inputs.Count)
                        break;
                    inputs[1].Add(new Sample(br.ReadInt32()));
                }
                if (ctlEnabled[2])
                {
                    if (br.BaseStream.Position + sizeof(int) > lenFs || 2 < 0 || 2 >= inputs.Count)
                        break;
                    inputs[2].Add(new Sample(br.ReadInt32()));
                }
                if (ctlEnabled[3])
                {
                    if (br.BaseStream.Position + sizeof(int) > lenFs || 3 < 0 || 3 >= inputs.Count)
                        break;
                    inputs[3].Add(new Sample(br.ReadInt32()));
                }
            }

            br.Close();
            if (closeFileStream) fs.Close();

            if (inputs == null) return (null, Sentiment.Fail);

            return (inputs, Sentiment.Success);
        }

        public static (Sentiment Sentiment, Interaction.UIError? Error, List<string> notifications) SaveMovie(FileStream fs, MovieHeader header, ObservableCollection<ObservableCollection<Sample>> inputs)
        {
            BinaryWriter br = new BinaryWriter(fs);
            List<string> notifications = new();

            // this is reused from Mupen Utilities
            byte[] zeroar1 = new byte[160]; byte[] zeroar2 = new byte[56];
            byte[] magic = new byte[4] { 0x4D, 0x36, 0x34, 0x1A };
            Array.Clear(zeroar1, 0, zeroar1.Length);
            Array.Clear(zeroar2, 0, zeroar2.Length);

            br.Write(header.Magic); // Int32 - Magic (4D36341A)
            br.Write(header.Version); // UInt32 - Version number (3)
            br.Write(header.UID); // UInt32 - UID
            br.Write((UInt32)header.LengthVIs); // UInt32 - VIs
            br.Write((UInt32)header.Rerecords); // UInt32 - RRs
            br.Write(header.VIsPerSecond); // Byte - VI/s
            br.Write(header.Controllers); // Byte - Controllers
            br.Write((Int16)0); // 2 Bytes - RESERVED
            br.Write(header.LengthSamples); // UInt32 - Input Samples

            br.Write((UInt16)header.StartFlags); // UInt16 - Movie start type
            br.Write((Int16)0); // 2 bytes - RESERVED
            br.Write(header.ControllerFlags.Raw); // UInt32 - Controller Flags
            br.Write(zeroar1, 0, zeroar1.Length); // 160 bytes - RESERVED
            if (header.RomName.Length > 32)
                notifications.Add(String.Format(Properties.Resources.TooLongStringNotificationFormat, Properties.Resources.RomName));
            byte[] romname = new byte[32];
            romname = ASCIIEncoding.ASCII.GetBytes(header.RomName);
            Array.Resize(ref romname, 32);
            br.Write(romname, 0, 32);
            br.Write(header.RomCRC);
            br.Write(header.RomCountry);
            br.Write(zeroar2, 0, zeroar2.Length); // 56 bytes - RESERVED


            byte[] gfx = new byte[64];
            byte[] audio = new byte[64];
            byte[] input = new byte[64];
            byte[] rsp = new byte[64];
            byte[] author = new byte[222];
            byte[] desc = new byte[256];

            
            if (header.VideoPluginName.Length > 64)
                notifications.Add(String.Format(Properties.Resources.TooLongStringNotificationFormat, Properties.Resources.VideoPluginName));
            if (header.AudioPluginName.Length > 64)
                notifications.Add(String.Format(Properties.Resources.TooLongStringNotificationFormat, Properties.Resources.AudioPluginName));
            if (header.InputPluginName.Length > 64)
                notifications.Add(String.Format(Properties.Resources.TooLongStringNotificationFormat, Properties.Resources.InputPluginName));
            if (header.RSPPluginName.Length > 64)
                notifications.Add(String.Format(Properties.Resources.TooLongStringNotificationFormat, Properties.Resources.RSPPluginName));
            if (header.Author.Length > 222)
                notifications.Add(String.Format(Properties.Resources.TooLongStringNotificationFormat, Properties.Resources.Author));
            if (header.Description.Length > 256)
                notifications.Add(String.Format(Properties.Resources.TooLongStringNotificationFormat, Properties.Resources.Description));

            gfx = Encoding.ASCII.GetBytes(header.VideoPluginName);
            audio = Encoding.ASCII.GetBytes(header.AudioPluginName);
            input = Encoding.ASCII.GetBytes(header.InputPluginName);
            rsp = Encoding.ASCII.GetBytes(header.RSPPluginName);
            author = Encoding.UTF8.GetBytes(header.Author);
            desc = Encoding.UTF8.GetBytes(header.Description);

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
                return (Sentiment.Fail, new Interaction.UIError(Properties.Resources.SaveAlignment, Properties.Resources.SaveAlignment), notifications);
            }

            int controllers = 0;
            for (int i = 0; i < 4; i++)
                if (BitopHelper.GetBit(header.ControllerFlags.Raw, i)) controllers++;
            if(controllers == 0)
            {
                return (Sentiment.Fail, new Interaction.UIError(Properties.Resources.NoControllers, Properties.Resources.NoControllers), notifications);
            }
            
            br.BaseStream.Seek(1024, SeekOrigin.Begin);

            for (int i = 0; i < inputs[0].Count; i++)
            {
                if (BitopHelper.GetBit(header.ControllerFlags.Raw, 0))
                    br.Write(inputs[0][i].Raw);
                if (BitopHelper.GetBit(header.ControllerFlags.Raw, 1) && 1 < inputs.Count && i < inputs[3].Count)
                    br.Write(inputs[1][i].Raw);
                if (BitopHelper.GetBit(header.ControllerFlags.Raw, 2) && 2 < inputs.Count && i < inputs[3].Count)
                    br.Write(inputs[2][i].Raw);
                if (BitopHelper.GetBit(header.ControllerFlags.Raw, 3) && 3 < inputs.Count && i < inputs[3].Count)
                    br.Write(inputs[3][i].Raw);
            }

            return (Sentiment.Success, null, notifications);
        }

    }
}
