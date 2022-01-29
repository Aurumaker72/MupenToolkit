using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MupenToolkit.Core.Helper;
using MupenToolkit.Core.Interaction;
using MupenToolkit.Core.Movie;
using PostSharp.Patterns.Model;

namespace MupenToolkit.Core.UI
{
    [NotifyPropertyChanged]
    public class StateContainer : INotifyPropertyChanged
    {
        public StateContainer()
        {
            LoadMovie = new LoadMovieCommand { mwv = this };
            UnloadMovie = new UnloadMovieCommand { mwv = this };
            SaveMovie = new SaveMovieCommand { mwv = this };
            EditControllerFlags = new EditControllerFlagsCommand { mwv = this };
            EditCountry = new EditCountryCommand { mwv = this };
            InputStatistics = new InputStatisticsCommand { mwv = this };
            CountryChanged = new CountryChangedCommand { mwv = this };
            DiagnoseMovie = new MovieDiagnosisCommand { mwv = this };
            BypassMovie = new BypassMovieCommand { mwv = this };
            LoadLastMovie = new LoadLastMovieCommand { mwv = this };
            SampleIndexChange = new SampleIndexChangeCommand { mwv = this };
            JoystickClick = new JoystickClickCommand { mwv=this};
            CanvasClickCommand = new RelayCommand(ExecuteCanvasClickCommand, CanExecuteCanvasClickCommand);

        }
        public Movie.MovieHeader Header { get; set; } = new();
        public Movie.InputContainer Input { get; set; } = new();

        public int CurrentController { get; set; } = 0;
        public bool Busy { get; set; } = false;
        public bool FileLoaded { get; set; } = false;
        public UIError Error { get; set; } = new();
        public string Mode { get; set; } = "None";
        public Provider.InfoProvider.InteractionTypes InteractionMode { get; set; } = Provider.InfoProvider.InteractionTypes.None;
        public ObservableCollection<Statistic> Statistics { get; set; } = new();
        public ObservableCollection<Statistic> Diagnosis { get; set; } = new();
        public int CurrentSampleIndex { get; set; }

        public Sample _SelectedSample;
        public Sample SelectedSample
        {
            get {
                if (CurrentController < Input.Samples.Count && CurrentSampleIndex < Input.Samples[CurrentController].Count)
                    return Input.Samples[CurrentController][CurrentSampleIndex];
                else
                    return null; // TODO: ??? Dont just throw null bomb
            }

        }


        private RelayCommand _canvasClickCommand;

        public RelayCommand CanvasClickCommand
        {
            get { return _canvasClickCommand; }
            set
            {
                _canvasClickCommand = value;
                OnPropertyChanged();
            }
        }

        public bool CanExecuteCanvasClickCommand(object parameter)
        {
            return true;
        }

        public void ExecuteCanvasClickCommand(object parameter)
        {
            var canvas = parameter as Canvas;
            Point p = Mouse.GetPosition(canvas);

            SelectedSample.X = (sbyte)MathHelper.Clamp(Math.Round(p.X), -127, 127);
            SelectedSample.Y = (sbyte)MathHelper.Clamp(Math.Round(p.Y), -127, 127);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }





        public ICommand LoadMovie { get; set; }
        public ICommand LoadLastMovie { get; set; }
        public ICommand UnloadMovie { get; set; }
        public ICommand SaveMovie { get; set; }
        public ICommand EditControllerFlags { get; set; }
        public ICommand EditCountry { get; set; }
        public ICommand InputStatistics { get; set; }
        public ICommand CountryChanged { get; set; }
        public ICommand DiagnoseMovie { get; set; }
        public ICommand BypassMovie { get; set; }
        public ICommand SampleIndexChange { get; set; }
        public ICommand JoystickClick { get; set; }
        

        
    }
}
