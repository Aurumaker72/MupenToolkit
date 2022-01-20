using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Movie
{
    [NotifyPropertyChanged]
    public class Statistic
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }

        public Statistic()
        {
        }

        public Statistic(string name, string description, string value)
        {
            Name = name;
            Description = description;
            Value = value;
        }

        
    }

    
}
