using MupenToolkit.Core.Helper;
using MupenToolkit.Core.Movie;
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

namespace MupenToolkit
{
    public partial class MainWindow : Window
    {
        public static StateContainer stateContainer = new();

        public MainWindow()
        {
            InitializeComponent();
            stateContainer.FileLoaded = false;

            this.DataContext = stateContainer;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            // inplausible with MVVM, we have to resort to events
            string[] fNames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            string fName = fName = fNames[0];
            if(fNames.Length > 1)
            {
                MessageBox.Show(Properties.Resources.DragDropTooMany, Title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (!PathHelper.ValidPath(fName, "m64"))
            {
                stateContainer.Error.Message = MupenToolkit.Properties.Resources.NotAMovie;
                stateContainer.Error.Visible ^= true;
                return;
            }
            MovieManager.LoadMovie(stateContainer, fName);


        }

        private void Border_Joystick_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point p = Mouse.GetPosition(Canvas_Joystick);
                stateContainer.SelectedSample.X = (sbyte)MathHelper.Clamp(Math.Round(p.X), -127, 127);
                stateContainer.SelectedSample.Y = (sbyte)MathHelper.Clamp(Math.Round(p.Y), -127, 127);
            }

        }
    }
}
