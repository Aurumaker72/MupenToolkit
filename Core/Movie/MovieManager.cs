using MupenToolkit.Core.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
                header.ControllerFlags = br.ReadUInt32();
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
                    ctlEnabled[i] = BitopHelper.GetBit(header.ControllerFlags, i);
                    if (ctlEnabled[i])
                    {
                        inputs.Add(new ObservableCollection<Sample>());
                    }
                }
                while (curFrame < frames)
                {
                    if (ctlEnabled[0])
                    {
                        if (br.BaseStream.Position + 4 > lenFs)
                            break;
                        inputs[0].Add(new Sample(br.ReadInt32()));
                    }
                    if (ctlEnabled[1])
                    {
                        if (br.BaseStream.Position + 4 > lenFs)
                            break;
                        inputs[1].Add(new Sample(br.ReadInt32()));
                    }
                    if (ctlEnabled[2])
                    {
                        if (br.BaseStream.Position + 4 > lenFs)
                            break;
                        inputs[2].Add(new Sample(br.ReadInt32()));
                    }
                    if (ctlEnabled[3])
                    {
                        if (br.BaseStream.Position + 4 > lenFs)
                            break;
                        inputs[3].Add(new Sample(br.ReadInt32()));
                    }
                }

                br.Close();
                if (closeFileStream) fs.Close();

                return (inputs, Sentiment.Success);
        }

    }
}
