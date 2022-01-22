using MupenToolkit.Core.Helper;
using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Movie
{
    public enum ButtonsIndicies
    {
        DPadRight,
        DPadLeft,
        DPadDown,
        DPadUp,
        Start,
        Z,
        B,
        A,
        CPadRight,
        CPadLeft,
        CPadDown,
        CPadUp,
        R,
        L,
    }
    public enum JoystickIndicies
    {
        X,
        Y,
    }

    //[NotifyPropertyChanged]
    public class Sample
    {
        public int Raw;

        public bool _DPadRight;
        public bool DPadRight
        {
            get
            {
                return _DPadRight;
            }
            set
            {
                _DPadRight = value;
                BitopHelper.SetBit(ref Raw, _DPadRight, 0);
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }


        public bool _DPadLeft;
        public bool DPadLeft
        {
            get
            {
                return _DPadLeft;
            }
            set
            {
                _DPadLeft = value;
                BitopHelper.SetBit(ref Raw, _DPadLeft, 1);
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }

        public bool _DPadDown;
        public bool DPadDown
        {
            get
            {
                return _DPadDown;
            }
            set
            {
                _DPadDown = value;
                BitopHelper.SetBit(ref Raw, _DPadDown, 2);
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }

        public bool _DPadUp;
        public bool DPadUp
        {
            get
            {
                return _DPadUp;
            }
            set
            {
                _DPadUp = value;
                BitopHelper.SetBit(ref Raw, _DPadUp, 3);
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }


        public bool _Start;
        public bool Start
        {
            get
            {
                return _Start;
            }
            set
            {
                _Start = value;
                BitopHelper.SetBit(ref Raw, _Start, 4);
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }

        public bool _Z;
        public bool Z
        {
            get
            {
                return _Z;
            }
            set
            {
                _Z = value;
                BitopHelper.SetBit(ref Raw, _Z, 5);
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }


        public bool _B;
        public bool B
        {
            get
            {
                return _B;
            }
            set
            {
                _B = value;
                BitopHelper.SetBit(ref Raw, _B, 6);
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }


        public bool _A;
        public bool A
        {
            get
            {
                return _A;
            }
            set
            {
                _A = value;
                BitopHelper.SetBit(ref Raw, _A, 7);
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }


        public bool _CPadRight;
        public bool CPadRight
        {
            get
            {
                return _CPadRight;
            }
            set
            {
                _CPadRight = value;
                BitopHelper.SetBit(ref Raw, _CPadRight, 8);
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }


        public bool _CPadLeft;
        public bool CPadLeft
        {
            get
            {
                return _CPadLeft;
            }
            set
            {
                _CPadLeft = value;
                BitopHelper.SetBit(ref Raw, _CPadLeft, 9);
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }

        public bool _CPadDown;
        public bool CPadDown
        {
            get
            {
                return _CPadDown;
            }
            set
            {
                _CPadDown = value;
                BitopHelper.SetBit(ref Raw, _CPadDown, 10);
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }


        public bool _CPadUp;
        public bool CPadUp
        {
            get
            {
                return _CPadUp;
            }
            set
            {
                _CPadUp = value;
                BitopHelper.SetBit(ref Raw, _CPadUp, 11);
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }

        public bool _R;
        public bool R
        {
            get
            {
                return _R;
            }
            set
            {
                _R = value;
                BitopHelper.SetBit(ref Raw, _R, 12);
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }

        public bool _L;
        public bool L
        {
            get
            {
                return _L;
            }
            set
            {
                _L = value;
                BitopHelper.SetBit(ref Raw, _L, 13);
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }

        private readonly bool _Reserved1;
        private readonly bool _Reserved2;

        public sbyte _X;
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
                    fixed (int* ptr = &Raw)
                        BitopHelper.SetByte(ptr, _X, 2);
                }
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }

        public sbyte _Y;
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
                    fixed(int* ptr = &Raw)
                        BitopHelper.SetByte(ptr, _Y, 3);
                }
                MainWindow.stateContainer.Statistics = InputStatistics.GetStatistics();
                OnPropertyChanged();
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public Sample(int rawValue)
        {
            Set(rawValue);
        }

        /// <summary>
        /// Parses raw value and pushes it to fields
        /// </summary>
        /// <param name="rawValue">Raw controller bitfield</param>
        public void Set(int rawValue)
        {
            DPadRight = Core.Helper.BitopHelper.GetBit(rawValue, 0);
            DPadLeft = Core.Helper.BitopHelper.GetBit(rawValue, 1);
            DPadDown = Core.Helper.BitopHelper.GetBit(rawValue, 2);
            DPadUp = Core.Helper.BitopHelper.GetBit(rawValue, 3);
            Start = Core.Helper.BitopHelper.GetBit(rawValue, 4);
            Z = Core.Helper.BitopHelper.GetBit(rawValue, 5);
            B = Core.Helper.BitopHelper.GetBit(rawValue, 6);
            A = Core.Helper.BitopHelper.GetBit(rawValue, 7);
            CPadRight = Core.Helper.BitopHelper.GetBit(rawValue, 8);
            CPadLeft = Core.Helper.BitopHelper.GetBit(rawValue, 9);
            CPadDown = Core.Helper.BitopHelper.GetBit(rawValue, 10);
            CPadUp = Core.Helper.BitopHelper.GetBit(rawValue, 11);
            R = Core.Helper.BitopHelper.GetBit(rawValue, 12);
            L = Core.Helper.BitopHelper.GetBit(rawValue, 13);

            X = Core.Helper.BitopHelper.GetSByte(rawValue, 2);
            Y = Core.Helper.BitopHelper.GetSByte(rawValue, 3);

            Raw = rawValue;
        }



        public bool Get(ButtonsIndicies button)
        {
            // this is very efficient
            Set(Raw);
            switch (button)
            {
                case ButtonsIndicies.DPadRight:
                    return DPadRight;

                case ButtonsIndicies.DPadLeft:
                    return DPadLeft;

                case ButtonsIndicies.DPadDown:
                    return DPadDown;

                case ButtonsIndicies.DPadUp:
                    return DPadUp;

                case ButtonsIndicies.Start:
                    return Start;

                case ButtonsIndicies.Z:
                    return Z;

                case ButtonsIndicies.B:
                    return B;

                case ButtonsIndicies.A:
                    return A;

                case ButtonsIndicies.CPadRight:
                    return CPadRight;

                case ButtonsIndicies.CPadLeft:
                    return CPadLeft;

                case ButtonsIndicies.CPadDown:
                    return CPadDown;

                case ButtonsIndicies.CPadUp:
                    return CPadUp;

                case ButtonsIndicies.R:
                    return R;

                case ButtonsIndicies.L:
                    return L;

                default:
                    throw new ArgumentException($"Invalid input \'{button.ToString()}\'");
            }
        }

        public int Get(JoystickIndicies button)
        {
            // this is very efficient
            Set(Raw);
            switch (button)
            {
                case JoystickIndicies.X:
                    return X;

                case JoystickIndicies.Y:
                    return Y;

                default:
                    throw new ArgumentException($"Invalid input \'{button.ToString()}\'");
            }
        }

    }
}
