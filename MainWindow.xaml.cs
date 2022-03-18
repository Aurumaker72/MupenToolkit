using MaterialDesignThemes.Wpf;
using MupenToolkitPRE.MVVM;
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
    }
}
