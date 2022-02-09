using MupenToolkit.Core.Helper;
using MupenToolkit.Core.Movie;
using MupenToolkit.Core.Provider;
using MupenToolkit.Core.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MupenToolkit
{
    public partial class MainWindow : Window
    {
        public static StateContainer stateContainer = new();
        private readonly DispatcherTimer MovieTimer = new DispatcherTimer();



        public MainWindow()
        {
            LocalizationProvider.Initialize();

            InitializeComponent();
            stateContainer.FileLoaded = false;

            this.DataContext = stateContainer;

            MovieTimer = new DispatcherTimer(DispatcherPriority.Background);
            MovieTimer.Interval = TimeSpan.FromMilliseconds(1000 / 30);
            MovieTimer.Tick += new EventHandler(this.MovieTimer_Tick);
            MovieTimer.Start();
        }

        private void MovieTimer_Tick(object sender, EventArgs? e)
        {
            if (stateContainer.Movie.Resumed && stateContainer.FileLoaded)
            {
                //stateContainer.SampleIndexChange.Execute("1"); 
                // TODO: Is it safe to touch VM from code-behind
                int i = 1;
                if (stateContainer.CurrentController > stateContainer.Input.Samples.Count || stateContainer.CurrentController < 0) return;
                if (stateContainer.CurrentSampleIndex + i < 0 || stateContainer.CurrentSampleIndex >= stateContainer.Input.Samples[stateContainer.CurrentController].Count-1) return;
                stateContainer.CurrentSampleIndex += i;
            }
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            // inplausible with MVVM, we have to resort to events
            string[] fNames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            string fName = fName = fNames[0];
            if (fNames.Length > 1)
            {
                MessageBox.Show(Properties.Resources.DragDropTooMany, Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (!PathHelper.ValidPath(fName))
            {
                stateContainer.Error.Message = MupenToolkit.Properties.Resources.NotAMovie;
                stateContainer.Error.Visible ^= true;
                return;
            }
            MovieManager.AttemptLoad(stateContainer, fName);


        }

        private void Border_Joystick_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (stateContainer.SelectedSample == null) return;
                Point p = Mouse.GetPosition(Canvas_Joystick);
                var tgX = (sbyte)MathHelper.Clamp(Math.Round(p.X), -127, 127);
                var tgY = (sbyte)MathHelper.Clamp(Math.Round(p.Y), -127, 127);
                if (tgX < 7 && tgX > -7)
                    tgX = 0;
                if (tgY < 7 && tgY > -7)
                    tgY = 0;
                stateContainer.SelectedSample.X = tgX;
                stateContainer.SelectedSample.Y = tgY;
            }

        }
    }
}
