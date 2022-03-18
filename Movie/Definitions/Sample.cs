using Microsoft.Toolkit.Mvvm.ComponentModel;
using MupenToolkitPRE.LowLevel;
using System;

namespace MupenToolkitPRE.Movie.Definitions
{
    public class Sample : ObservableObject
    {
        protected int _Raw;
        public int Raw
        {
            get { return _Raw; }
            set
            {
                _Raw = value;
                OnPropertyChanged();
            }
        }

        protected bool _DPadRight;
        public bool DPadRight
        {
            get
            {
                return _DPadRight;
            }
            set
            {
                _DPadRight = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _DPadRight, 0);
                OnPropertyChanged();
            }
        }


        protected bool _DPadLeft;
        public bool DPadLeft
        {
            get
            {
                return _DPadLeft;
            }
            set
            {
                _DPadLeft = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _DPadLeft, 1);
                OnPropertyChanged();
            }
        }

        protected bool _DPadDown;
        public bool DPadDown
        {
            get
            {
                return _DPadDown;
            }
            set
            {
                _DPadDown = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _DPadDown, 2);
                OnPropertyChanged();
            }
        }

        protected bool _DPadUp;
        public bool DPadUp
        {
            get
            {
                return _DPadUp;
            }
            set
            {
                _DPadUp = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _DPadUp, 3);
                OnPropertyChanged();
            }
        }


        protected bool _Start;
        public bool Start
        {
            get
            {
                return _Start;
            }
            set
            {
                _Start = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _Start, 4);
                OnPropertyChanged();
            }
        }

        protected bool _Z;
        public bool Z
        {
            get
            {
                return _Z;
            }
            set
            {
                _Z = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _Z, 5);
                OnPropertyChanged();
            }
        }


        protected bool _B;
        public bool B
        {
            get
            {
                return _B;
            }
            set
            {
                _B = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _B, 6);
                OnPropertyChanged();
            }
        }


        protected bool _A;
        public bool A
        {
            get
            {
                return _A;
            }
            set
            {
                _A = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _A, 7);
                OnPropertyChanged();
            }
        }


        protected bool _CPadRight;
        public bool CPadRight
        {
            get
            {
                return _CPadRight;
            }
            set
            {
                _CPadRight = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _CPadRight, 8);
                OnPropertyChanged();
            }
        }


        protected bool _CPadLeft;
        public bool CPadLeft
        {
            get
            {
                return _CPadLeft;
            }
            set
            {
                _CPadLeft = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _CPadLeft, 9);
                OnPropertyChanged();
            }
        }

        protected bool _CPadDown;
        public bool CPadDown
        {
            get
            {
                return _CPadDown;
            }
            set
            {
                _CPadDown = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _CPadDown, 10);
                OnPropertyChanged();
            }
        }


        protected bool _CPadUp;
        public bool CPadUp
        {
            get
            {
                return _CPadUp;
            }
            set
            {
                _CPadUp = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _CPadUp, 11);
                OnPropertyChanged();
            }
        }

        protected bool _R;
        public bool R
        {
            get
            {
                return _R;
            }
            set
            {
                _R = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _R, 12);
                OnPropertyChanged();
            }
        }

        protected bool _L;
        public bool L
        {
            get
            {
                return _L;
            }
            set
            {
                _L = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _L, 13);
                OnPropertyChanged();
            }
        }

        protected bool _Reserved1;
        protected bool _Reserved2;
        public bool Reserved1
        {
            get { return _Reserved1; }
            set
            {
                _Reserved1 = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _Reserved1, 14);
            }
        }
        public bool Reserved2
        {
            get { return _Reserved2; }
            set
            {
                _Reserved2 = value;
                Raw = BitOperationsHelper.SetBit(_Raw, _Reserved2, 15);
            }
        }
        protected sbyte _X;
        public sbyte X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
                unsafe
                {
                    fixed (int* ptr = &_Raw)
                        BitOperationsHelper.SetByte(ptr, _X, 2);
                }
                Raw = _Raw; // hack: update
                OnPropertyChanged();
            }
        }

        protected sbyte _Y;
        public sbyte Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
                unsafe
                {
                    fixed (int* ptr = &_Raw)
                        BitOperationsHelper.SetByte(ptr, _Y, 3);
                }
                Raw = _Raw; // hack: update
                OnPropertyChanged();
            }
        }

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
