using BlackJackApp.Models;
using System.Collections.ObjectModel;

namespace BlackJackApp.ViewModels
{
    public class GameStateManager
    {
        private readonly Player _player;
        private readonly Dealer _dealer;

        public bool CanStart { get; private set; }
        public bool CanHit { get; private set; }
        public bool CanStand { get; private set; }

        public string PlayerStatus { get; private set; }
        public string DealerStatus { get; private set; }
        public string PotAmount { get; private set; }
        public string PlayerBalance { get; private set; }
        public string DealerBalance { get; private set; }

        public void SetCanHit(bool value)
        {
            CanHit = value;
        }

        public void SetCanStart(bool value)
        {
            CanStart = value;
        }

        public void SetCanStand(bool value)
        {
            CanStand = value;
        }

        public GameStateManager(Player player, Dealer dealer)
        {
            _player = player;
            _dealer = dealer;
            ResetGameState();
            UpdateUI();
        }

        public void ResetGameState()
        {
            CanStart = true;
            CanHit = false;
            CanStand = false;
        }

        public void SetGameInProgressState()
        {
            CanStart = false;
            CanHit = true;
            CanStand = true;
        }

        public void UpdateUI(int playerScore = 0, int dealerScore = 0)
        {
            PlayerStatus = $"Очки игрока: {playerScore}";
            DealerStatus = $"Очки дилера: {dealerScore}";
            PotAmount = $"Пот: {_player.Bet + _dealer.Bet}";
            PlayerBalance = $"Баланс игрока: {_player.Balance}";
            DealerBalance = $"Баланс дилера: {_dealer.Balance}";
        }

        public void SetStatusMessage(string message)
        {
            PotAmount = message;
        }
    }
}