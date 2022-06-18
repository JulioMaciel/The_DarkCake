using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AussieCake.Util
{
    public static class Footer
    {
        private static TextBlock LabelFooter = ((MainWindow)Application.Current.MainWindow).lblFooter;
        private static Button BtnFooter = ((MainWindow)Application.Current.MainWindow).btnShowDetails;

        private static TaskScheduler TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();

        public static void Log(string msg)
        {
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    LabelFooter.Foreground = Brushes.Black;
            //    LabelFooter.Text = msg;
            //});

            Task.Factory.StartNew(() =>
            {
                LabelFooter.Foreground = Brushes.Black;
                LabelFooter.Text = msg;
            }, 
            CancellationToken.None, TaskCreationOptions.None, TaskScheduler);
        }

        public static void LogError(string msg)
        {
            Task.Factory.StartNew(() =>
            {
                BtnFooter.Visibility = Visibility.Collapsed;

                LabelFooter.Foreground = Brushes.DarkRed;
                LabelFooter.Text = msg;
            },
            CancellationToken.None, TaskCreationOptions.None, TaskScheduler);
        }

        public static void LogError(string msg, object details)
        {
            LogError(msg);

            Task.Factory.StartNew(() =>
            {
                BtnFooter.CleanClickEvents();
                BtnFooter.Visibility = Visibility.Visible;

                BtnFooter.Click += (e,sender) => MessageBox.Show(details.ToString(), "Error Details");
            },
            CancellationToken.None, TaskCreationOptions.None, TaskScheduler);
        }
    }
}