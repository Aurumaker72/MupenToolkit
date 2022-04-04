using MaterialDesignThemes.Wpf;
using MupenToolkitPRE.Movie.Helper;
using MupenToolkitPRE.MVVM;
using System.Diagnostics;
using System.Windows;

namespace MupenToolkitPRE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainViewModel MainVM;

        public MainWindow()
        {
            InitializeComponent();

            MainVM = new(MainSnackbar);
            this.DataContext = MainVM;
            MainVM.ChangeThemeCommand.Execute(Properties.Settings.Default.DarkTheme);
            MainVM.ChangeCultureCommand.Execute(Properties.Settings.Default.Culture);


        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
        public static void SetTheme(bool Dark)
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(Dark ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);
            Properties.Settings.Default.DarkTheme = Dark;
        }

        // pain in the ass to do while respecting MVVM principle :(
        private void Window_Drop(object sender, DragEventArgs e)
        {
            var fNames = (string[])e.Data.GetData(DataFormats.FileDrop, true);

            // windows should allegedly prevent this due to event ordering but i dont believe it one bit!!!!!!!
            if (fNames.Length != 1 || !fNames[0].ValidMoviePath())
            {
                return;
            }
            else
            {
                MovieLoadHelper.Load(MainVM, fNames[0]);
            }
        }

        // this is slow as hell and also uses evil event propagation and ordering hack
        private void Window_DragOver(object sender, DragEventArgs e)
        {
            var fNames = (string[])e.Data.GetData(DataFormats.FileDrop, true);
            e.Effects = fNames.Length != 1 || !fNames[0].ValidMoviePath() ? DragDropEffects.None : DragDropEffects.Copy;
            e.Handled = true;
        }
    }
}
