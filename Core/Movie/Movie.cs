using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MupenToolkit.Core.Movie
{
    [NotifyPropertyChanged]
    public class Movie : INotifyPropertyChanged
    {
        public MovieHeader Header { get; set; } = new();
        /// <summary>
        /// Whether this movie is in play-back mode
        /// We store this per-movie to avoid inconsistency
        /// </summary>
        public bool Resumed { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
