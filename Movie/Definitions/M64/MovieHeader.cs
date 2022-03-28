using Microsoft.Toolkit.Mvvm.ComponentModel;
using PostSharp.Patterns.Model;

namespace MupenToolkitPRE.Movie.Definitions.M64
{
    [NotifyPropertyChanged]
    public class MovieHeader : ObservableObject
    {
        /// <summary>
        /// Unique magic bytes
        /// First 4 bytes of file
        /// expected value is 4D 36 34 1A. Anything else is invalid and not a movie
        /// </summary>
        public uint Magic { get; set; }
        /// <summary>
        /// M64 standard version
        /// Should be 3 on all releases since 0.5
        /// </summary>
        public uint Version { get; set; }
        /// <summary>
        /// Unique identifier which must not be modified externally
        /// </summary>
        public uint UID { get; set; }

        /// <summary>
        /// Movie length in VIs
        /// It is impossible to determine samples from this by dividing total VIs by VI/s due to interrupt inconsistency
        /// </summary>
        public uint LengthVIs { get; set; }
        /// <summary>
        /// Rerecords
        /// </summary>
        public uint Rerecords { get; set; }
        /// <summary>
        /// VIs per second
        /// Pretty sure these depend on AC frequency of N64 region (so 50 or 60hz)
        /// </summary>
        public byte VIsPerSecond { get; set; }
        private readonly ushort _Reserved1;
        /// <summary>
        /// Movie length in logical frames
        /// </summary>
        public uint LengthSamples { get; set; }
        /// <summary>
        /// Movie length determined by file size
        /// </summary>
        public uint LengthFile { get; set; }
        /// <summary>
        /// Movie start flags
        /// 1 - Savestate
        /// 2 - Power-on
        /// 4 - EEPROM
        /// Other - Unknown
        /// </summary>
        public ushort StartFlags { get; set; }
        private ushort _Reserved2;
        public ControllersBitflag ControllerFlags { get; set; } = new(0);
        private readonly uint _ReservedFlags;

        private readonly string _OldAuthorInfo;
        private readonly string _OldDescription;
        public string RomName { get; set; } // internal rom name
        /// <summary>
        /// Unique ROM CRC. Can be used to determine specific game
        /// </summary>
        public uint RomCRC { get; set; }
        /// <summary>
        /// Rom Country Code
        /// </summary>
        public ushort RomCountry { get; set; }
        private readonly string _Reserved3;
        /// <summary>
        /// ASCII Encoded string describing the Video Plugin Name
        /// </summary>
        public string VideoPluginName { get; set; }
        /// <summary>
        /// ASCII Encoded string describing the Audio Plugin Name
        /// </summary>
        public string AudioPluginName { get; set; }
        /// <summary>
        /// ASCII Encoded string describing the Input Plugin Name
        /// </summary>
        public string InputPluginName { get; set; }
        /// <summary>
        /// ASCII Encoded string describing the RSP Plugin Name
        /// </summary>
        public string RSPPluginName { get; set; }
        /// <summary>
        /// UTF-8 Encoded string describing the Author
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// UTF-8 Encoded string describing the Description
        /// </summary>
        public string Description { get; set; }
    }
}
