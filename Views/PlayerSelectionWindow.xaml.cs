using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using BlackJackApp.Data;
using BlackJackApp.Models;
using BlackJackApp.ViewModels;

namespace BlackJackApp.Views
{
    public partial class PlayerSelectionWindow : Window
    {
        private List<Player>? _players;
        public Player? SelectedPlayer { get; private set; }

        public PlayerSelectionWindow()
        {
            InitializeComponent();
            LoadPlayers();
        }

        private void LoadPlayers()
        {
            _players = new List<Player>();
            using (var reader = DatabaseHelper.ExecuteReader("SELECT PlayerID, Name, Balance FROM Players"))
            {
                while (reader.Read())
                {
                    _players.Add(new Player(reader.GetString(1))
                    {
                        PlayerID = reader.GetInt32(0),
                        Balance = reader.GetInt32(2)
                    });
                }
            }
            PlayersComboBox.ItemsSource = _players;
        }

        private void CreateNewPlayer_Click(object sender, RoutedEventArgs e)
        {
            string newPlayerName = NewPlayerName.Text;
            if (string.IsNullOrWhiteSpace(newPlayerName) || newPlayerName == "Введите имя нового игрока")
            {
                MessageBox.Show("Введите имя игрока.");
                return;
            }

            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Name", newPlayerName),
                new SqlParameter("@Balance", 1000)
            };

            DatabaseHelper.ExecuteNonQuery("INSERT INTO Players (Name, Balance) VALUES (@Name, @Balance)", parameters);
            LoadPlayers();
            MessageBox.Show("Игрок успешно создан.");
        }

        private void SelectPlayer_Click(object sender, RoutedEventArgs e)
        {
            SelectedPlayer = (Player)PlayersComboBox.SelectedItem;
            if (SelectedPlayer == null)
            {
                MessageBox.Show("Выберите игрока.");
                return;
            }

            var mainWindow = new MainWindow
            {
                DataContext = new MainViewModel(SelectedPlayer)
            };

            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();

            this.Close();
        }

        private void NewPlayerName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NewPlayerName.Text == "Введите имя нового игрока")
            {
                NewPlayerName.Text = "";
            }
        }

        private void NewPlayerName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewPlayerName.Text))
            {
                NewPlayerName.Text = "Введите имя нового игрока";
            }
        }
    }
}