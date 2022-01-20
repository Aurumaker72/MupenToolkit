using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MupenToolkit.Core.Interaction;
using MupenToolkit.Core.Movie;
using PostSharp.Patterns.Model;

namespace MupenToolkit.Core.UI
{
    [NotifyPropertyChanged]
    public class StateContainer
    {
        public Movie.MovieHeader Header { get; set; } = new();
        public Movie.InputContainer Input { get; set; } = new();
        public int CurrentController { get; set; } = 0;
        public bool Busy { get; set; } = false;
        public bool FileLoaded { get; set; } = false;
        public UIError Error { get; set; } = new();
        public string Mode { get; set; } = "None"; // this is horrible!
        public ObservableCollection<Statistic> Statistics { get; set; } = new();

        public ICommand LoadMovie { get; set; }
        public ICommand UnloadMovie { get; set; }
        public ICommand SaveMovie { get; set; }
        public ICommand EditControllerFlags { get; set; }
        public ICommand EditCountry { get; set; }
        public ICommand InputStatistics { get; set; }
        public ICommand CountryChanged { get; set; }

        public StateContainer()
        {
            LoadMovie = new LoadMovieCommand { mwv = this };
            UnloadMovie = new UnloadMovieCommand { mwv = this };
            SaveMovie = new SaveMovieCommand { mwv = this };
            EditControllerFlags = new EditControllerFlagsCommand { mwv = this };
            EditCountry = new EditCountryCommand { mwv = this };
            InputStatistics = new InputStatisticsCommand { mwv = this };
            CountryChanged = new CountryChangedCommand { mwv = this };

        }
    }
}
