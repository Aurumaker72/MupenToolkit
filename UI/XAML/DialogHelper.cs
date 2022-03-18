using MaterialDesignThemes.Wpf;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MupenToolkitPRE.UI.XAML
{
    public static class DialogHelper
    {
        public static void ShowDialog(string message)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                DialogHost.Show(new GenericDialog
                {
                    Message = { Text = message }
                }, "RootDialog");
            }));
        }
        public static void ShowSnackbar(Snackbar snackbar, string message)
        {
            if (snackbar == null) throw new ArgumentNullException("VM Snackbar");
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
