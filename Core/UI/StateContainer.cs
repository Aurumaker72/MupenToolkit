using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MupenToolkit.Core.Interaction;
using PostSharp.Patterns.Model;

namespace MupenToolkit.Core.UI
{
    [NotifyPropertyChanged]
    public class StateContainer
    {
        public Movie.MovieHeader Header { get; set; } = new();
        public Movie.InputContainer Input { get; set; } = new();
        public int CurrentController { get; set; } = 0;
        public bool Busy { get; set; } = true;
        public bool FileLoaded { get; set; } = false;
        public UIError Error { get; set; } = new();

        public ICommand LoadMovie { get; set; }
        public ICommand UnloadMovie { get; set; }
        public ICommand SaveMovie { get; set; }
        public ICommand TASStudioEdit { get; set; }

        public StateContainer()
        {
            LoadMovie = new LoadMovieCommand { mwv = this };
            UnloadMovie = new UnloadMovieCommand { mwv = this };
            SaveMovie = new SaveMovieCommand { mwv = this };
            TASStudioEdit = new TASStudioEditCommand { mwv = this };
        }
    }
}
