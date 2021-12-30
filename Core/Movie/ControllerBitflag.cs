using MupenToolkit.Core.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Movie
{
    public class ControllersBitflag
    {
        public uint Raw;

        // this is horrible

        private bool _Controller0Connected;
        private bool _Controller0Mempak;
        private bool _Controller0Rumblepak;
        private bool _Controller1Connected;
        private bool _Controller1Mempak;
        private bool _Controller1Rumblepak;
        private bool _Controller2Connected;
        private bool _Controller2Mempak;
        private bool _Controller2Rumblepak;
        private bool _Controller3Connected;
        private bool _Controller3Mempak;
        private bool _Controller3Rumblepak;

        public bool Controller0Connected
        {
            get
            {
                return _Controller0Connected;
            }
            set
            {
                _Controller0Connected = value;
                BitopHelper.SetBit(ref Raw, _Controller0Connected, 0);
                OnPropertyChanged();
            }
        }

        public bool Controller0Mempak
        {
            get
            {
                return _Controller0Mempak;
            }
            set
            {
                _Controller0Mempak = value;
                BitopHelper.SetBit(ref Raw, _Controller0Mempak, 1);
                OnPropertyChanged();
            }
        }
        public bool Controller0Rumblepak
        {
            get
            {
                return _Controller0Rumblepak;
            }
            set
            {
                _Controller0Rumblepak = value;
                BitopHelper.SetBit(ref Raw, _Controller0Rumblepak, 2);
                OnPropertyChanged();
            }
        }
        public bool Controller1Connected
        {
            get
            {
                return _Controller1Connected;
            }
            set
            {
                _Controller1Connected = value;
                BitopHelper.SetBit(ref Raw, _Controller1Connected, 3);
                OnPropertyChanged();
            }
        }
        public bool Controller1Mempak
        {
            get
            {
                return _Controller1Mempak;
            }
            set
            {
                _Controller1Mempak = value;
                BitopHelper.SetBit(ref Raw, _Controller1Mempak, 4);
                OnPropertyChanged();
            }
        }
        public bool Controller1Rumblepak
        {
            get
            {
                return _Controller1Rumblepak;
            }
            set
            {
                _Controller1Rumblepak = value;
                BitopHelper.SetBit(ref Raw, _Controller1Rumblepak, 5);
                OnPropertyChanged();
            }
        }
        public bool Controller2Connected
        {
            get
            {
                return _Controller2Connected;
            }
            set
            {
                _Controller2Connected = value;
                BitopHelper.SetBit(ref Raw, _Controller2Connected, 6);
                OnPropertyChanged();
            }
        }
        public bool Controller2Mempak
        {
            get
            {
                return _Controller2Mempak;
            }
            set
            {
                _Controller2Mempak = value;
                BitopHelper.SetBit(ref Raw, _Controller2Mempak, 7);
                OnPropertyChanged();
            }
        }
        public bool Controller2Rumblepak
        {
            get
            {
                return _Controller2Rumblepak;
            }
            set
            {
                _Controller2Rumblepak = value;
                BitopHelper.SetBit(ref Raw, _Controller2Rumblepak, 8);
                OnPropertyChanged();
            }
        }
        public bool Controller3Connected
        {
            get
            {
                return _Controller3Connected;
            }
            set
            {
                _Controller3Connected = value;
                BitopHelper.SetBit(ref Raw, _Controller3Connected, 9);
                OnPropertyChanged();
            }
        }
        public bool Controller3Mempak
        {
            get
            {
                return _Controller3Mempak;
            }
            set
            {
                _Controller3Mempak = value;
                BitopHelper.SetBit(ref Raw, _Controller3Mempak, 10);
                OnPropertyChanged();
            }
        }
        public bool Controller3Rumblepak
        {
            get
            {
                return _Controller3Rumblepak;
            }
            set
            {
                _Controller3Rumblepak = value;
                BitopHelper.SetBit(ref Raw, _Controller3Rumblepak, 11);
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public ControllersBitflag(uint raw)
        {
            Set(raw);
        }


        /// <summary>
        /// Parses raw value and pushes it to fields
        /// </summary>
        /// <param name="rawValue">Raw controller bitfield</param>
        public void Set(uint rawValue)
        {
            Controller0Connected = Core.Helper.BitopHelper.GetBit(rawValue, 0);
            Controller0Mempak    = Core.Helper.BitopHelper.GetBit(rawValue, 1);
            Controller0Rumblepak = Core.Helper.BitopHelper.GetBit(rawValue, 2);

            Controller1Connected = Core.Helper.BitopHelper.GetBit(rawValue, 3);
            Controller1Mempak    = Core.Helper.BitopHelper.GetBit(rawValue, 4);
            Controller1Rumblepak = Core.Helper.BitopHelper.GetBit(rawValue, 5);

            Controller2Connected = Core.Helper.BitopHelper.GetBit(rawValue, 6);
            Controller2Mempak    = Core.Helper.BitopHelper.GetBit(rawValue, 7);
            Controller2Rumblepak = Core.Helper.BitopHelper.GetBit(rawValue, 8);

            Controller3Connected = Core.Helper.BitopHelper.GetBit(rawValue, 9);
            Controller3Mempak    = Core.Helper.BitopHelper.GetBit(rawValue, 10);
            Controller3Rumblepak = Core.Helper.BitopHelper.GetBit(rawValue, 11);

            Raw = rawValue;
        }

    }
}
