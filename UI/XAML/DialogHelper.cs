using MaterialDesignThemes.Wpf;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MupenToolkitPRE.UI.XAML
{
    public static class DialogHelper
    {
        public static void ShowDialogIf(string message, string title, bool condition)
        {
            if (condition) ShowDialog(message, title);
        }
        public static void ShowDialog(string message, string title)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                DialogHost.Show(new GenericDialog
                {
                    Message = { Text = message },
                    Title = { Text = title }
                }, "RootDialog");
            }));
        }
        public static void ShowSnackbar(Snackbar snackbar, string message)
        {
            if (snackbar == null) throw new ArgumentNullException("VM Snackbar missing");
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Task.Factory.StartNew(() => Thread.Sleep(0)).ContinueWith(t =>
                {
                    snackbar.MessageQueue?.Enqueue(message);
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }));

        }
    }
}
