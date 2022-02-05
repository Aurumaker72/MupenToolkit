using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Movie
{
    [NotifyPropertyChanged]
    public class Combo : INotifyPropertyChanged
    {
        public Combo(string name, int length)
        {
            Name = name;
            Length = length;
        }

        public string Name { get; set; }
        public int Length { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
