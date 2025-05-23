using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using BlackJackApp.Data;
using BlackJackApp.Models;

namespace BlackJackApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Player _player;
        private readonly Dealer _dealer;
        private readonly GameManager _gameManager;
        private readonly GameStateManager _stateManager;

        private string _coinImage = "pack://application:,,,/Images/coin.gif";
        private string _playerName;
        private int[] _availableBets = { 50, 100, 200 };
        private int _selectedBet = 100;

        public ObservableCollection<Card> PlayerHand { get; }
        public ObservableCollection<Card> DealerHand { get; }

        public string CoinImage
        {
            get => _coinImage;
            set => SetField(ref _coinImage, value);
        }

        public string PlayerName
        {
            get => _playerName;
            set => SetField(ref _playerName, value);
        }

        public string PlayerStatus
        {
            get => _stateManager.PlayerStatus;
            private set => OnPropertyChanged();
        }

        public string DealerStatus
        {
            get => _stateManager.DealerStatus;
            private set => OnPropertyChanged();
        }

        public string PotAmount
        {
            get => _stateManager.PotAmount;
            private set => OnPropertyChanged();
        }

        public string PlayerBalance
        {
            get => _stateManager.PlayerBalance;
            private set => OnPropertyChanged();
        }

        public string DealerBalance
        {
            get => _stateManager.DealerBalance;
            private set => OnPropertyChanged();
        }

        public bool CanStart
        {
            get => _stateManager.CanStart;
            private set => OnPropertyChanged();
        }

        public bool CanHit
        {
            get => _stateManager.CanHit;
            private set => OnPropertyChanged();
        }

        public bool CanStand
        {
            get => _stateManager.CanStand;
            private set => OnPropertyChanged();
        }

        public int[] AvailableBets
        {
            get => _availableBets;
            set => SetField(ref _availableBets, value);
        }

        public int SelectedBet
        {
            get => _selectedBet;
            set => SetField(ref _selectedBet, value);
        }

        public ICommand StartNewGameCommand { get; private set; }
        public ICommand HitCommand { get; private set; }
        public ICommand StandCommand { get; private set; }

        public MainViewModel(Player selectedPlayer)
        {
            if (selectedPlayer == null)
                throw new ArgumentNullException(nameof(selectedPlayer), "Выбранный игрок не может быть null.");

            _player = selectedPlayer;
            _dealer = new Dealer("Дилер");
            _gameManager = new GameManager(_player, _dealer);
            _stateManager = new GameStateManager(_player, _dealer);

            PlayerHand = new ObservableCollection<Card>();
            DealerHand = new ObservableCollection<Card>();

            PlayerName = $"Игрок: {_player.Name}";
            StartNewGameCommand = new RelayCommand(async () => await StartNewGame(), () => _stateManager.CanStart);
            HitCommand = new RelayCommand(async () => await Hit(), () => _stateManager.CanHit);
            StandCommand = new RelayCommand(async () => await Stand(), () => _stateManager.CanStand);

            AdjustAvailableBets();
        }

        private void AdjustAvailableBets()
        {
            if (_player.Balance < 200)
            {
                AvailableBets = _player.Balance >= 100 ? new[] { 50, 100 } : new[] { 50 };
                SelectedBet = AvailableBets[0];
            }
        }

        private async Task StartNewGame()
        {
            if (_player.Balance < SelectedBet)
            {
                _stateManager.SetStatusMessage($"Недостаточно средств для ставки {SelectedBet}");
                return;
            }

            try
            {
                _gameManager.PlaceBets(SelectedBet);
                PlayerHand.Clear();
                DealerHand.Clear();

                PlayerHand.Add(_gameManager.DrawCard());
                UpdateGameState();

                DealerHand.Add(await _gameManager.DrawCardWithDelay(500));
                UpdateGameState();
                await Task.Delay(500);

                PlayerHand.Add(await _gameManager.DrawCardWithDelay(500));
                UpdateGameState();

                _stateManager.SetGameInProgressState();
                NotifyStateChanges();
            }
            catch (InvalidOperationException ex)
            {
                _stateManager.SetStatusMessage(ex.Message);
                NotifyStateChanges();
            }
        }

        private async Task Hit()
        {
            PlayerHand.Add(await _gameManager.DrawCardWithDelay(500));
            UpdateGameState();

            if (_player.Score > 21)
            {
                await Task.Delay(1000);
                HandleGameOver("Игрок проиграл! Перебор!", GameOutcome.PlayerBust);
            }
            else if (_player.Score == 21)
            {
                _stateManager.SetCanHit(false);
                NotifyStateChanges();
            }
        }

        private async Task Stand()
        {
            _stateManager.ResetGameState();
            NotifyStateChanges();

            while (_dealer.Score < 17)
            {
                DealerHand.Add(await _gameManager.DrawCardWithDelay(1000));
                UpdateGameState();
            }

            var result = GameManager.DetermineResult(_player.Score, _dealer.Score, _player.Bet + _dealer.Bet);
            HandleGameResult(result);
        }

        private void UpdateGameState()
        {
            _player.Score = GameManager.CalculateScore(PlayerHand);
            _dealer.Score = GameManager.CalculateScore(DealerHand);
            _stateManager.UpdateUI(_player.Score, _dealer.Score);
            NotifyStateChanges();
        }

        private void HandleGameResult(GameResult result)
        {
            switch (result.Outcome)
            {
                case GameOutcome.PlayerWins:
                    _player.Balance += result.Amount;
                    _stateManager.SetStatusMessage("Игрок выиграл!");
                    break;
                case GameOutcome.DealerWins:
                    _dealer.Balance += result.Amount;
                    _stateManager.SetStatusMessage("Дилер выиграл!");
                    break;
                case GameOutcome.PlayerBust:
                    _dealer.Balance += result.Amount;
                    _stateManager.SetStatusMessage("Игрок проиграл! Перебор!");
                    break;
                case GameOutcome.DealerBust:
                    _player.Balance += result.Amount;
                    _stateManager.SetStatusMessage("Дилер проиграл! Перебор!");
                    break;
                case GameOutcome.Push:
                    _player.Balance += _player.Bet;
                    _dealer.Balance += _dealer.Bet;
                    _stateManager.SetStatusMessage("Ничья!");
                    break;
            }

            UpdatePlayerBalance();
            _stateManager.ResetGameState();
            NotifyStateChanges();
        }

        private void HandleGameOver(string message, GameOutcome outcome)
        {
            var result = new GameResult(outcome, _player.Bet + _dealer.Bet);
            HandleGameResult(result);
        }

        private void UpdatePlayerBalance()
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@PlayerID", _player.PlayerID),
                new SqlParameter("@Balance", _player.Balance)
            };
            DatabaseHelper.ExecuteNonQuery("UPDATE Players SET Balance = @Balance WHERE PlayerID = @PlayerID", parameters);
        }

        private void NotifyStateChanges()
        {
            OnPropertyChanged(nameof(PlayerStatus));
            OnPropertyChanged(nameof(DealerStatus));
            OnPropertyChanged(nameof(PotAmount));
            OnPropertyChanged(nameof(PlayerBalance));
            OnPropertyChanged(nameof(DealerBalance));
            OnPropertyChanged(nameof(CanStart));
            OnPropertyChanged(nameof(CanHit));
            OnPropertyChanged(nameof(CanStand));

            CommandManager.InvalidateRequerySuggested();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }
    }
}