using MupenToolkitPRE.LowLevel;
using MupenToolkitPRE.Movie.Manager;
using PostSharp.Patterns.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace MupenToolkitPRE.Movie.Definitions.M64
{
    [NotifyPropertyChanged]
    public class Movie : BaseN64Storage
    {
        public MovieHeader Header { get; set; } = new();
        protected MovieHeader HeaderUnchanged { get; set; } = new();

        public override string Extension => "m64";


        public override (InteractionStatus Status, ObservableCollection<ObservableCollection<Sample>>? Samples) Load(string path)
        {
            FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader br = new(fs);

            var headerStatus = LoadHeader(br);

            if (!headerStatus.Status.Success)
                return (headerStatus.Status, null);

            var samplesStatus = LoadInputs(br, headerStatus.Header!);

            if (!samplesStatus.Status.Success)
                return (samplesStatus.Status, default);

            this.Header = this.HeaderUnchanged = headerStatus.Header!;

            var tmpName = System.IO.Path.GetFileNameWithoutExtension(path);
            this.Name = string.Concat(tmpName[..1].ToUpper(), tmpName[1..]);

            br.Close();
            fs.Close();

            return (new(true), samplesStatus.Samples);
        }

        public override InteractionStatus Save(string path, ObservableCollection<ObservableCollection<Sample>> inputs)
        {
            if (this.Header == null) return new(false, Properties.Resources.GenericError);

            int controllers = 0;
            for (int i = 0; i < 4; i++)
                if (BitOperationsHelper.GetBit(this.Header.ControllerFlags.Raw, i)) controllers++;
            if (controllers == 0)
            {
                return new(false, Properties.Resources.InsufficientControllers);
            }

            FileStream fs = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Write);
            BinaryWriter br = new(fs);

            byte[] zeroar1 = new byte[160]; byte[] zeroar2 = new byte[56];
            byte[] magic = new byte[4] { 0x4D, 0x36, 0x34, 0x1A };
            Array.Clear(zeroar1, 0, zeroar1.Length);
            Array.Clear(zeroar2, 0, zeroar2.Length);

            br.Write(this.Header.Magic); // Int32 - Magic (4D36341A)
            br.Write(this.Header.Version); // UInt32 - Version number (3)
            br.Write(this.Header.UID); // UInt32 - UID
            br.Write((UInt32)this.Header.LengthVIs); // UInt32 - VIs
            br.Write((UInt32)this.Header.Rerecords); // UInt32 - RRs
            br.Write(this.Header.VIsPerSecond); // Byte - VI/s
            br.Write(this.Header.ControllerFlags.Controllers); // Byte - Controllers
            br.Write((Int16)0); // 2 Bytes - RESERVED
            br.Write(this.Header.LengthSamples); // UInt32 - Input Samples

            br.Write((UInt16)this.Header.StartFlags); // UInt16 - Movie start type
            br.Write((Int16)0); // 2 bytes - RESERVED
            br.Write(this.Header.ControllerFlags.Raw); // UInt32 - Controller Flags
            br.Write(zeroar1, 0, zeroar1.Length); // 160 bytes - RESERVED
            byte[] romname = new byte[32];
            romname = ASCIIEncoding.ASCII.GetBytes(this.Header.RomName);
            Array.Resize(ref romname, 32);
            br.Write(romname, 0, 32);
            br.Write(this.Header.RomCRC);
            br.Write(this.Header.RomCountry);
            br.Write(zeroar2, 0, zeroar2.Length); // 56 bytes - RESERVED


            byte[] gfx = new byte[64];
            byte[] audio = new byte[64];
            byte[] input = new byte[64];
            byte[] rsp = new byte[64];
            byte[] author = new byte[222];
            byte[] desc = new byte[256];


            gfx = Encoding.ASCII.GetBytes(this.Header.VideoPluginName);
            audio = Encoding.ASCII.GetBytes(this.Header.AudioPluginName);
            input = Encoding.ASCII.GetBytes(this.Header.InputPluginName);
            rsp = Encoding.ASCII.GetBytes(this.Header.RSPPluginName);
            author = Encoding.UTF8.GetBytes(this.Header.Author);
            desc = Encoding.UTF8.GetBytes(this.Header.Description);

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
                return new(false, Properties.Resources.SavingMisalignment);
            }



            br.BaseStream.Seek(1024, SeekOrigin.Begin);

            for (int i = 0; i < inputs[0].Count; i++)
            {
                if (BitOperationsHelper.GetBit(this.Header.ControllerFlags.Raw, 0))
                    br.Write(inputs[0][i].Raw);
                if (BitOperationsHelper.GetBit(this.Header.ControllerFlags.Raw, 1) && 1 < inputs.Count && i < inputs[3].Count)
                    br.Write(inputs[1][i].Raw);
                if (BitOperationsHelper.GetBit(this.Header.ControllerFlags.Raw, 2) && 2 < inputs.Count && i < inputs[3].Count)
                    br.Write(inputs[2][i].Raw);
                if (BitOperationsHelper.GetBit(this.Header.ControllerFlags.Raw, 3) && 3 < inputs.Count && i < inputs[3].Count)
                    br.Write(inputs[3][i].Raw);
            }
            br.Close();
            fs.Close();
            return new(true);
        }

        protected override (InteractionStatus Status, MovieHeader? Header) LoadHeader(BinaryReader br)
        {
            br.BaseStream.Seek(0, SeekOrigin.Begin);
            if (br.BaseStream.Length < 1024)
            {
                br.Close();
                return (new(false, Properties.Resources.TooShort), null);
            }
            MovieHeader header = new();

            try
            {
                header.Magic = br.ReadUInt32();
                header.Version = br.ReadUInt32();
                header.UID = br.ReadUInt32();
                header.LengthVIs = br.ReadUInt32();
                header.Rerecords = br.ReadUInt32();
                header.VIsPerSecond = br.ReadByte();
                header.ControllerFlags.Controllers = br.ReadByte();
                br.ReadBytes(2);
                header.LengthSamples = br.ReadUInt32();
                header.StartFlags = br.ReadUInt16();
                br.ReadBytes(2);
                header.ControllerFlags = new ControllersBitflag(br.ReadUInt32());
                br.ReadBytes(160);
                header.RomName = br.ReadBytes(32).ASCIIToStringTrimmed();
                header.RomCRC = br.ReadUInt32();
                header.RomCountry = br.ReadUInt16();
                br.ReadBytes(56);

                header.VideoPluginName = br.ReadBytes(64).ASCIIToStringTrimmed();
                header.AudioPluginName = br.ReadBytes(64).ASCIIToStringTrimmed();
                header.InputPluginName = br.ReadBytes(64).ASCIIToStringTrimmed();
                header.RSPPluginName = br.ReadBytes(64).ASCIIToStringTrimmed();

                header.Author = br.ReadBytes(222).UTF8ToStringTrimmed();
                header.Description = br.ReadBytes(256).UTF8ToStringTrimmed();
                string errorMessage = string.Empty;

                if (header.Magic != 0x4D36341A && header.Magic != 439629389)
                {
                    errorMessage += Properties.Resources.WrongMagic + Environment.NewLine;
                }
                if (header.Version != 3)
                {
                    errorMessage += Properties.Resources.OutdatedVersion + Environment.NewLine;
                }
                //if (header.UID == 0 || header.RomCRC == 0)
                //{
                //    errorMessage += Properties.Resources.InvalidUID + Environment.NewLine;
                //}
                if (header.LengthVIs == 0)
                {
                    errorMessage += Properties.Resources.InsufficientVIs + Environment.NewLine;
                }
                if (header.VIsPerSecond != 50 && header.VIsPerSecond != 60)
                {
                    errorMessage += Properties.Resources.InvalidVIsPerSecond + Environment.NewLine;
                }
                if (errorMessage != String.Empty)
                {
                    br.Close();
                    errorMessage = errorMessage.Trim().TrimEnd(Environment.NewLine.ToCharArray());
                    return (new(false, errorMessage), null);
                }
                return (new(true), header);
            }
            catch
            {
                return (new(false, Properties.Resources.GenericError), null);
            }
        }

        protected override (InteractionStatus Status, ObservableCollection<ObservableCollection<Sample>>? Samples) LoadInputs(BinaryReader br, MovieHeader header)
        {
            if (br.BaseStream.Position != 1024)
            {
                return (new(false, Properties.Resources.SamplesLoadingMisalignment), null);
            }

            br.BaseStream.Seek(1024, SeekOrigin.Begin);

            // INFO: We need to retouch header here
            header.LengthFile = ((uint)br.BaseStream.Length - 1024) / 4;

            ObservableCollection<ObservableCollection<Sample>> inputs = new();
            if (header.ControllerFlags.Controller0Connected) inputs.Add(new());
            if (header.ControllerFlags.Controller1Connected) inputs.Add(new());
            if (header.ControllerFlags.Controller2Connected) inputs.Add(new());
            if (header.ControllerFlags.Controller3Connected) inputs.Add(new());

            long curFrame = 0;
            while (curFrame < br.BaseStream.Length)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (BitOperationsHelper.GetBit(header.ControllerFlags.Raw, i))
                    {
                        if (br.BaseStream.Position + sizeof(int) < br.BaseStream.Length && i < inputs.Count)
                            inputs[i].Add(new Sample(br.ReadInt32()));
                    }
                }
                curFrame++;
            }

            return (new(true), inputs);
        }
    }
}
