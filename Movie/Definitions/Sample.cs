using Microsoft.Toolkit.Mvvm.ComponentModel;
using MupenToolkitPRE.LowLevel;
using System;

namespace MupenToolkitPRE.Movie.Definitions
{

    // !!!
    // this class is absolutely performance and memory critical
    // i had to rethink the hierarchy for this and sacrifice some readability
    // 
    // HACK 1 - use INPC attribute instead of inheriting from ObservableObject; this avoids 1 (2?) eventhandlers being created
    // HACK 2 - use undocumented IncludeAdditionalHelperMethods flag to not create ONPC wrappers (e.g.: SetProperty)
    // POTENTIAL HACK 3 - Pass pointer to raw inside SetBit then copy afterward to avoid further stack interaction
    [INotifyPropertyChanged(IncludeAdditionalHelperMethods = false)]
    public partial class Sample
    {
        [ObservableProperty]
        private int raw;

        public bool DPadRight
        {
            get => BitOperationsHelper.GetBit(Raw, 0);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 0);
                OnPropertyChanged();
            }
        }


        public bool DPadLeft
        {
            get => BitOperationsHelper.GetBit(Raw, 1);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 1);
                OnPropertyChanged();
            }
        }

        public bool DPadDown
        {
            get => BitOperationsHelper.GetBit(Raw, 2);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 2);
                OnPropertyChanged();
            }
        }

        public bool DPadUp
        {
            get => BitOperationsHelper.GetBit(Raw, 3);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 3);
                OnPropertyChanged();
            }
        }


        public bool Start
        {
            get => BitOperationsHelper.GetBit(Raw, 4);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 4);
                OnPropertyChanged();
            }
        }

        public bool Z
        {
            get => BitOperationsHelper.GetBit(Raw, 5);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 5);
                OnPropertyChanged();
            }
        }


        public bool B
        {
            get => BitOperationsHelper.GetBit(Raw, 6);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 6);
                OnPropertyChanged();
            }
        }


        public bool A
        {
            get => BitOperationsHelper.GetBit(Raw, 7);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 7);
                OnPropertyChanged();
            }
        }

        public bool CPadRight
        {
            get => BitOperationsHelper.GetBit(Raw, 8);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 8);
                OnPropertyChanged();
            }
        }


        public bool CPadLeft
        {
            get => BitOperationsHelper.GetBit(Raw, 9);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 9);
                OnPropertyChanged();
            }
        }

        public bool CPadDown
        {
            get => BitOperationsHelper.GetBit(Raw, 10);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 10);
                OnPropertyChanged();
            }
        }

        public bool CPadUp
        {
            get => BitOperationsHelper.GetBit(Raw, 11);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 11);
                OnPropertyChanged();
            }
        }

        public bool R
        {
            get => BitOperationsHelper.GetBit(Raw, 12);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 12);
                OnPropertyChanged();
            }
        }

        public bool L
        {
            get => BitOperationsHelper.GetBit(Raw, 13);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 13);
                OnPropertyChanged();
            }
        }

        public bool Reserved1
        {
            get => BitOperationsHelper.GetBit(Raw, 14);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 14);
                OnPropertyChanged();
            }
        }
        public bool Reserved2
        {
            get => BitOperationsHelper.GetBit(Raw, 15);
            set
            {
                Raw = BitOperationsHelper.SetBit(Raw, value, 15);
                OnPropertyChanged();
            }
        }

        public sbyte X
        {
            get => BitOperationsHelper.GetSByte(Raw, 2);
            set
            {
                BitOperationsHelper.SetByte(ref raw, value, 2);
                Raw = raw;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Degree));
            }
        }

        public sbyte Y
        {
            get => BitOperationsHelper.GetSByte(Raw, 3);
            set
            {
                BitOperationsHelper.SetByte(ref raw, value, 3);
                Raw = raw;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Degree));
            }
        }

        public double Degree => Math.Atan2(Y, X) * (180 / Math.PI);

        public Sample(int rawValue)
        {
            Set(rawValue);
            OnPropertyChanged();
        }

        public bool GetButtonByIndex(int index)
        {
            if (index > 15) throw new ArgumentException("Invalid button index!");
            return BitOperationsHelper.GetBit(Raw, index);
        }

        /// <summary>
        /// Parses raw value and pushes it to fields
        /// </summary>
        /// <param name="rawValue">Raw controller bitfield</param>
        public void Set(int rawValue)
        {
            DPadRight = BitOperationsHelper.GetBit(rawValue, 0);
            DPadLeft = BitOperationsHelper.GetBit(rawValue, 1);
            DPadDown = BitOperationsHelper.GetBit(rawValue, 2);
            DPadUp = BitOperationsHelper.GetBit(rawValue, 3);
            Start = BitOperationsHelper.GetBit(rawValue, 4);
            Z = BitOperationsHelper.GetBit(rawValue, 5);
            B = BitOperationsHelper.GetBit(rawValue, 6);
            A = BitOperationsHelper.GetBit(rawValue, 7);
            CPadRight = BitOperationsHelper.GetBit(rawValue, 8);
            CPadLeft = BitOperationsHelper.GetBit(rawValue, 9);
            CPadDown = BitOperationsHelper.GetBit(rawValue, 10);
            CPadUp = BitOperationsHelper.GetBit(rawValue, 11);
            R = BitOperationsHelper.GetBit(rawValue, 12);
            L = BitOperationsHelper.GetBit(rawValue, 13);
            Reserved1 = BitOperationsHelper.GetBit(rawValue, 14);
            Reserved2 = BitOperationsHelper.GetBit(rawValue, 15);

            X = BitOperationsHelper.GetSByte(rawValue, 2);
            Y = (sbyte)(BitOperationsHelper.GetSByte(rawValue, 3) * -1);

            Raw = rawValue;
        }



    }
}
