using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Movie
{
    [NotifyPropertyChanged]
    public class InputContainer
    {
        

        public ObservableCollection<ObservableCollection<Sample>> Samples { get; set; } = new();
    }
}
