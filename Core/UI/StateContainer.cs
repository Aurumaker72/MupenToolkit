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
        public ICommand LoadData { get; set; }
        public ICommand LoadLast { get; set; }
        public ICommand UnloadData { get; set; }
        public ICommand SaveData { get; set; }
        public ICommand EditControllerFlags { get; set; }
        public ICommand EditCountry { get; set; }
        public ICommand InputStatistics { get; set; }
        public ICommand CountryChanged { get; set; }
        public ICommand DiagnoseData { get; set; }
        public ICommand BypassMovie { get; set; }
        public ICommand SampleIndexChange { get; set; }
        public ICommand JoystickClick { get; set; }

        public StateContainer()
        {
            LoadData = new LoadDataCommand { mwv = this };
            UnloadData = new UnloadDataCommand { mwv = this };
            SaveData = new SaveDataCommand { mwv = this };
            EditControllerFlags = new EditControllerFlagsCommand { mwv = this };
            EditCountry = new EditCountryCommand { mwv = this };
            InputStatistics = new InputStatisticsCommand { mwv = this };
            CountryChanged = new CountryChangedCommand { mwv = this };
            DiagnoseData = new DataDiagnosisCommand { mwv = this };
            BypassMovie = new BypassMovieCommand { mwv = this };
            LoadLast = new LoadLastCommand { mwv = this };
            SampleIndexChange = new SampleIndexChangeCommand { mwv = this };
            JoystickClick = new JoystickClickCommand { mwv=this};
            CanvasClickCommand = new RelayCommand(ExecuteCanvasClickCommand, CanExecuteCanvasClickCommand);

        }
        /// <summary>
        /// Storage class for M64
        /// </summary>
        public Movie.Movie Movie { get; set; } = new();
        /// <summary>
        /// Storage class for CMB
        /// </summary>
        public List<Movie.Combo> Combos { get; set; } = new();
        /// <summary>
        /// Global input container for inputs interaction and saving
        /// </summary>
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

        public Sample SelectedSample
        {
            get
            {

                if (CurrentController < Input.Samples.Count && CurrentSampleIndex < Input.Samples[CurrentController].Count)
                    return Input.Samples[CurrentController][CurrentSampleIndex];
                else
                    return null; // TODO: error handling

            }
        }

        public Combo SelectedCombo
        {
            get
            {
                if (InteractionMode != Provider.InfoProvider.InteractionTypes.Combo)
                {
                    //throw new ArgumentException("Invalid program state");
                    return null;
                }

                if (CurrentController < Combos.Count && CurrentController >= 0)
                    return Combos[CurrentController];
                else return null;// TODO: error handling
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





       
        

        
    }
}
