using System.Windows;
using BlackJackApp.Views;

namespace BlackJackApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow mainWindow = new MainWindow
            {
                DataContext = new ViewModels.MainViewModel()
            };
            mainWindow.Show();
        }
    }
}