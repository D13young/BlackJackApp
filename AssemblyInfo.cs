using System.Windows;
using BlackJackApp.ViewModels;
using BlackJackApp.Views;

namespace WpfApplication
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow mainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
            mainWindow.Show();
        }
    }
}