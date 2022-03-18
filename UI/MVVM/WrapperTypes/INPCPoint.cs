using Microsoft.Toolkit.Mvvm.ComponentModel;
using PostSharp.Patterns.Model;

namespace MupenToolkitPRE.UI.MVVM.WrapperTypes
{
    [NotifyPropertyChanged]
    public class INPCPoint : ObservableObject
    {
        public INPCPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
        public INPCPoint()
        {
        }

        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        public bool Equals(INPCPoint pt)
        {
            return pt.X == this.X && pt.Y == this.Y;
        }
        public bool Equals(int x, int y)
        {
            return new INPCPoint(x, y).Equals(this);
        }
    }
}
