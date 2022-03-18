using Microsoft.Toolkit.Mvvm.ComponentModel;
using MupenToolkitPRE.LowLevel;

namespace MupenToolkitPRE.Movie.Definitions.M64
{
    public class ControllersBitflag : ObservableObject
    {
        /// <summary>
        /// |  Bits  |       Description        |
        /// |  0..4  | Controller [n] enabled   |
        /// |1,4,7,10| Controller [n] mempak    |
        /// |2,5,8,11| Controller [n] rumblepak |
        /// </summary>
        public uint Raw;

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
        private byte _Controllers;


        public byte Controllers
        {
            get
            {
                return _Controllers;
            }
            set
            {
                _Controllers = value;
                OnPropertyChanged();
            }
        }
        private void ComputeControllers()
        {
            byte ctl = 0;
            for (int i = 0; i < 4; i++)
            {
                if (BitOperationsHelper.GetBit(Raw, i)) ctl++;
            }
            Controllers = ctl;
        }

        public bool Controller0Connected
        {
            get
            {
                return _Controller0Connected;
            }
            set
            {
                _Controller0Connected = value;
                BitOperationsHelper.SetBit(ref Raw, _Controller0Connected, 0);
                ComputeControllers();
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
                BitOperationsHelper.SetBit(ref Raw, _Controller0Mempak, 4);
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
                BitOperationsHelper.SetBit(ref Raw, _Controller0Rumblepak, 8);
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
                BitOperationsHelper.SetBit(ref Raw, _Controller1Connected, 1);
                ComputeControllers();
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
                BitOperationsHelper.SetBit(ref Raw, _Controller1Mempak, 5);
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
                BitOperationsHelper.SetBit(ref Raw, _Controller1Rumblepak, 9);
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
                BitOperationsHelper.SetBit(ref Raw, _Controller2Connected, 2);
                ComputeControllers();
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
                BitOperationsHelper.SetBit(ref Raw, _Controller2Mempak, 6);
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
                BitOperationsHelper.SetBit(ref Raw, _Controller2Rumblepak, 10);
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
                BitOperationsHelper.SetBit(ref Raw, _Controller3Connected, 3);
                ComputeControllers();
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
                BitOperationsHelper.SetBit(ref Raw, _Controller3Mempak, 7);
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
                BitOperationsHelper.SetBit(ref Raw, _Controller3Rumblepak, 11);
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Initialize bitflag from raw value
        /// </summary>
        /// <param name="raw">Raw controller bitfield</param>
        public ControllersBitflag(uint rawValue)
        {
            Set(rawValue);
        }
        public ControllersBitflag()
        {
        }

        /// <summary>
        /// Parses raw value and pushes it to fields
        /// </summary>
        /// <param name="rawValue">Raw controller bitfield</param>
        public void Set(uint rawValue)
        {
            Controller0Connected = BitOperationsHelper.GetBit(rawValue, 0);
            Controller0Mempak = BitOperationsHelper.GetBit(rawValue, 1);
            Controller0Rumblepak = BitOperationsHelper.GetBit(rawValue, 2);

            Controller1Connected = BitOperationsHelper.GetBit(rawValue, 3);
            Controller1Mempak = BitOperationsHelper.GetBit(rawValue, 4);
            Controller1Rumblepak = BitOperationsHelper.GetBit(rawValue, 5);

            Controller2Connected = BitOperationsHelper.GetBit(rawValue, 6);
            Controller2Mempak = BitOperationsHelper.GetBit(rawValue, 7);
            Controller2Rumblepak = BitOperationsHelper.GetBit(rawValue, 8);

            Controller3Connected = BitOperationsHelper.GetBit(rawValue, 9);
            Controller3Mempak = BitOperationsHelper.GetBit(rawValue, 10);
            Controller3Rumblepak = BitOperationsHelper.GetBit(rawValue, 11);


            Raw = rawValue;

            ComputeControllers();

        }

    }
}
