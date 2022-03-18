using MaterialDesignThemes.Wpf;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using MupenToolkitPRE.LowLevel;
using MupenToolkitPRE.LowLevel.Wrapper;
using MupenToolkitPRE.Movie.Definitions;
using MupenToolkitPRE.Movie.Manager;
using MupenToolkitPRE.UI.MVVM.WrapperTypes;
using PostSharp.Patterns.Model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MupenToolkitPRE.MVVM
{

    [NotifyPropertyChanged]
    public class MainViewModel : ObservableObject
    {
        #region N64

        public Movie.Definitions.MovieTypes MovieType { get; set; }
        public Movie.Definitions.M64.Movie Movie { get; set; } = new();

        public ObservableCollection<ObservableCollection<Sample>> Samples { get; set; } = new();

        public int CurrentController { get; set; }
        private int _CurrentSampleIndex;
        public int CurrentSampleIndex
        {
            get
            {
                return _CurrentSampleIndex;
            }
            set
            {
                _CurrentSampleIndex = NumericHelper.ClampFrame(Samples[CurrentController].Count, value); //Math.Clamp(value, 0, Math.Max(0, Samples[CurrentController].Count - 1));
                OnPropertyChanged();
            }
        }
        public Sample? CurrentSample
        {
            get
            {
                if (CurrentController >= Samples.Count) return null;
                if (CurrentSampleIndex >= Samples[CurrentController].Count) return null;
                return Samples[CurrentController][CurrentSampleIndex];
            }
        }
        public ObservableCollection<Sample>? CurrentSampleList
        {
            get
            {
                if (Samples.Count == 0) return null;
                return Samples[Math.Clamp(CurrentController, 0, Samples.Count)];
            }
        }
        #region Statistics
        public ObservableCollection<Statistic> Statistics { get; set; } = new();
        public int StatisticsSelectedButtonIndex
        {
            get;
            set;
        }
        public INPCPoint StatisticsSelectedJoystickValue { get; set; } = new();
        public int StatisticsFoundFrame { get; set; }
        public bool StatisticsSearchSuccessful { get; set; } = true;
        #endregion
        #endregion


        #region Commands
        public ChangeThemeCommand ChangeThemeCommand { get; set; }
        public LoadCommand LoadCommand { get; set; }
        public SaveCommand SaveCommand { get; set; }
        public ChangeCultureCommand ChangeCultureCommand { get; set; }
        public FrameAdvanceCommand FrameAdvanceCommand { get; set; }
        public TogglePlayCommand TogglePlayCommand { get; set; }
        public BypassLoadCommand BypassLoadCommand { get; set; }
        public SeekButtonCommand SeekButtonCommand { get; set; }
        public SeekJoystickCommand SeekJoystickCommand { get; set; }
        public UnloadCommand UnloadCommand { get; set; }
        public CountryChangedCommand CountryChangedCommand { get; set; }
        public LoadLastCommand LoadLastCommand { get; set; }
        public UploadLogCommand UploadLogCommand { get; set; }
        #endregion

        #region UI
        public Snackbar MainSnackbar;
        private int _SelectedPageIndex;
        private PageItem _SelectedPage;

        public int SelectedPageIndex
        {
            get => _SelectedPageIndex;
            set
            {
                _SelectedPageIndex = value;
                if (PageItems[_SelectedPageIndex].OnNavigated != null) PageItems[_SelectedPageIndex].OnNavigated();
                OnPropertyChanged();
            }
        }
        public PageItem SelectedPage
        {
            get => _SelectedPage;
            set
            {
                _SelectedPage = value;
                if (_SelectedPage.OnNavigated != null) _SelectedPage.OnNavigated();
                OnPropertyChanged();

            }
        }
        public ObservableCollection<PageItem> PageItems { get; set; } = new();
        public bool FileLoaded { get; set; }
        #endregion

        public MainViewModel(Snackbar sb = null)
        {
            ChangeThemeCommand = new() { mvm = this };
            SaveCommand = new() { mvm = this };
            LoadCommand = new() { mvm = this };
            ChangeCultureCommand = new() { mvm = this };
            FrameAdvanceCommand = new() { mvm = this };
            TogglePlayCommand = new() { mvm = this };
            BypassLoadCommand = new() { mvm = this };
            SeekButtonCommand = new() { mvm = this };
            SeekJoystickCommand = new() { mvm = this };
            UnloadCommand = new() { mvm = this };
            CountryChangedCommand = new() { mvm = this };
            LoadLastCommand = new() { mvm = this };
            UploadLogCommand = new() { mvm = this };

            PageItems = new ObservableCollection<PageItem>(new[]
            {
                new PageItem(typeof(HomePage),
                this,
                CodeBehindPackIconWrapper.FromKind(PackIconKind.Home, 24),
                delegate{AnalogInputPage.MovieTimer.Stop();
                }),
                new PageItem(typeof(HeaderPage),
                this,
                CodeBehindPackIconWrapper.FromKind(PackIconKind.FormatListBulleted, 24),
                delegate{AnalogInputPage.MovieTimer.Stop();
                }),
                new PageItem(typeof(ControllerFlagsPage),
                this, CodeBehindPackIconWrapper.FromKind(PackIconKind.Controller, 24),
                delegate{AnalogInputPage.MovieTimer.Stop();
                }),
                new PageItem(typeof(AnalogInputPage),
                this, CodeBehindPackIconWrapper.FromKind(PackIconKind.ControllerSquare,   24),
                delegate{AnalogInputPage.MovieTimer.Start();
                }),
                new PageItem(typeof(StatisticsPage),
                this, CodeBehindPackIconWrapper.FromKind(PackIconKind.GraphBar,           24),
                delegate{AnalogInputPage.MovieTimer.Stop(); StatisticManager.PushStatistics();
                }),
                new PageItem(typeof(ROMCountryPage),
                this, CodeBehindPackIconWrapper.FromKind(PackIconKind.Flag,               24),
                delegate{AnalogInputPage.MovieTimer.Stop();
                }),
                new PageItem(typeof(SettingsPage),
                this, CodeBehindPackIconWrapper.FromKind(PackIconKind.Settings,           24),
                delegate{AnalogInputPage.MovieTimer.Stop();
                }),
            });

            MainSnackbar = sb;
        }

        public void RefreshPages()
        {
            PageItems[0].Name = Properties.Resources.Home;
            PageItems[1].Name = Properties.Resources.Header;
            PageItems[2].Name = Properties.Resources.ControllerFlags;
            PageItems[3].Name = Properties.Resources.AnalogInput;
            PageItems[4].Name = Properties.Resources.Statistics;
            PageItems[5].Name = Properties.Resources.Country;
            PageItems[6].Name = Properties.Resources.Settings;

        }

    }

    [NotifyPropertyChanged]
    public class PageItem : ObservableObject
    {
        private readonly Type _ContentType;
        private readonly object? _DataContext;
        private object? _Content;
        [SafeForDependencyAnalysis]
        public object? Content => _Content ??= CreateContent();

        public string Name { get; set; }
        public Thickness MarginRequirement { get; set; } = new(16);
        public Control Accompanist { get; set; }

        public Action? OnNavigated;

        public PageItem(Type contentType, object? dataContext, Control accompanist, Action onNavigated)
        {
            _ContentType = contentType;
            _DataContext = dataContext;
            OnNavigated = onNavigated;
            Accompanist = accompanist;
        }

        private object? CreateContent()
        {
            var content = Activator.CreateInstance(_ContentType);
            if (_DataContext != null && content is FrameworkElement element)
            {
                element.DataContext = _DataContext;
            }
            return content;
        }
    }
}
