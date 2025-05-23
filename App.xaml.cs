using System;
using System.Windows;
using BlackJackApp.ViewModels;
using BlackJackApp.Views;

namespace BlackJackApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.ShutdownMode = ShutdownMode.OnMainWindowClose;

            var playerSelectionWindow = new PlayerSelectionWindow();
            playerSelectionWindow.Closed += (sender, args) =>
            {
                if (playerSelectionWindow.SelectedPlayer == null)
                {
                    Shutdown();
                }
            };

            playerSelectionWindow.Show();
        }
    }
}