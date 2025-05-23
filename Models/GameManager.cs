using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BlackJackApp.Models
{
    public class GameManager
    {
        private readonly Deck _deck;
        private readonly Player _player;
        private readonly Dealer _dealer;

        public GameManager(Player player, Dealer dealer)
        {
            _player = player;
            _dealer = dealer;
            _deck = new Deck();
            _deck.Shuffle();
        }

        public async Task<Card> DrawCardWithDelay(int delayMs = 1000)
        {
            await Task.Delay(delayMs);
            return DrawCard();
        }

        public void DealInitialCards(ObservableCollection<Card> playerHand, ObservableCollection<Card> dealerHand)
        {
            playerHand.Clear();
            dealerHand.Clear();

            dealerHand.Add(DrawCard());

            for (int i = 0; i < 2; i++)
            {
                playerHand.Add(DrawCard());
            }
        }

        public Card DrawCard()
        {
            if (_deck.CardsRemaining() == 0)
            {
                _deck.ResetAndShuffle();
            }
            return _deck.DrawCard();
        }

        public static int CalculateScore(IEnumerable<Card> hand)
        {
            int score = 0;
            int aces = 0;

            foreach (var card in hand)
            {
                score += card.Value;
                if (card.Rank == "Ace") aces++;
            }

            while (score > 21 && aces > 0)
            {
                score -= 10;
                aces--;
            }

            return score;
        }

        public void PlaceBets(int amount)
        {
            _player.PlaceBet(amount);
            _dealer.PlaceBet(amount);
        }

        public static GameResult DetermineResult(int playerScore, int dealerScore, int betAmount)
        {
            if (playerScore > 21)
                return new GameResult(GameOutcome.PlayerBust, betAmount);

            if (dealerScore > 21)
                return new GameResult(GameOutcome.DealerBust, betAmount);

            if (playerScore > dealerScore)
                return new GameResult(GameOutcome.PlayerWins, betAmount);

            if (playerScore < dealerScore)
                return new GameResult(GameOutcome.DealerWins, betAmount);

            return new GameResult(GameOutcome.Push, betAmount);
        }
    }

    public enum GameOutcome
    {
        PlayerWins,
        DealerWins,
        PlayerBust,
        DealerBust,
        Push
    }

    public class GameResult
    {
        public GameOutcome Outcome { get; }
        public int Amount { get; }

        public GameResult(GameOutcome outcome, int amount)
        {
            Outcome = outcome;
            Amount = amount;
        }
    }
}