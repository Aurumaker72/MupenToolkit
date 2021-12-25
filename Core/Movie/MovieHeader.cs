using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Movie
{
    [NotifyPropertyChanged]
    public class MovieHeader
    {
        public uint Magic { get; set; }
        public uint Version { get; set; }
        public uint UID { get; set; }

        public uint LengthVIs { get; set; }
        public uint Rerecords { get; set; }
        public byte VIsPerSecond { get; set; }
        public byte Controllers { get; set; }
        private readonly ushort _Reserved1;
        public uint LengthSamples { get; set; }

        public ushort StartFlags { get; set; } // should equal 2 if the movie is from a clean start
        private ushort _Reserved2;
        public uint ControllerFlags { get; set; }
        private readonly uint _ReservedFlags;

        private readonly string _OldAuthorInfo;
        private readonly string _OldDescription;
        public string RomName { get; set; } // internal rom name
        public uint RomCRC { get; set; }
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
