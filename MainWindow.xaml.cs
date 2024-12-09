using BlackJackApp;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BlackjackApp;

public partial class MainWindow : Window
{
    private Player player;
    private Dealer dealer;
    private Deck deck;

    public MainWindow()
    {
        InitializeComponent();
        player = new Player("Игрок");
        dealer = new Dealer("Дилер");
        deck = new Deck();
        deck.Shuffle();
        StartNewGame();
    }

    private void StartNewGame()
    {
        player.Hand.Clear();
        dealer.Hand.Clear();
        player.Score = 0;
        dealer.Score = 0;

        if (player.Balance <= 0 || dealer.Balance <= 0)
        {
            MessageBox.Show("Игра окончена! У одного из игроков закончились средства.");
            return;
        }

        PlaceBets(100);
        DealCards();
        UpdateUI();
    }

    private void PlaceBets(int amount)
    {
        try
        {
            player.PlaceBet(amount);
            dealer.PlaceBet(amount);
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void DealCards()
    {
        player.Hand.Add(DrawCard());
        player.Hand.Add(DrawCard());
        dealer.Hand.Add(DrawCard());
        player.Score = CalculateScore(player.Hand);
        dealer.Score = CalculateScore(dealer.Hand);
    }

    private Card DrawCard()
    {
        if (deck.CardsRemaining() < 10)
        {
            deck.ResetAndShuffle();
        }
        return deck.DrawCard();
    }

    private static int CalculateScore(List<Card> hand)
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

    private void HitButton_Click(object sender, RoutedEventArgs e)
    {
        player.Hand.Add(DrawCard());
        player.Score = CalculateScore(player.Hand);
        UpdateUI();

        if (player.Score > 21)
        {
            dealer.Play(deck);
            UpdateUI();
            MessageBox.Show("Вы проиграли! У вас перебор.");
            dealer.WinBet();
            player.LoseBet();

        }
    }

    private void StandButton_Click(object sender, RoutedEventArgs e)
    {
        dealer.Play(deck);
        UpdateUI();
        DetermineWinner();
    }

    private void DetermineWinner()
    {
        if (player.Score > 21)
        {
            dealer.WinBet();
            player.LoseBet();
            MessageBox.Show("Игрок проиграл!");
        }
        else if (dealer.Score > 21 || player.Score > dealer.Score)
        {
            player.WinBet();
            dealer.LoseBet();
            MessageBox.Show("Игрок выиграл!");
        }
        else if (player.Score < dealer.Score)
        {
            dealer.WinBet();
            player.LoseBet();
            MessageBox.Show("Дилер выиграл!");
        }
        else
        {
            player.Balance += player.Bet;
            dealer.Balance += dealer.Bet;
            player.LoseBet();
            dealer.LoseBet();
            MessageBox.Show("Ничья!");
        }
    }

    private void UpdateUI()
    {
        PlayerHand.Children.Clear();
        foreach (var card in player.Hand)
        {
            Image cardImage = new Image
            {
                Source = new BitmapImage(new Uri($"pack://application:,,,/Images/{card.Rank}_of_{card.Suit}.png")),
                Width = 100,
                Height = 150,
                Margin = new Thickness(5)
            };
            PlayerHand.Children.Add(cardImage);
        }

        DealerHand.Children.Clear();
        foreach (var card in dealer.Hand)
        {
            Image cardImage = new Image
            {
                Source = new BitmapImage(new Uri($"pack://application:,,,/Images/{card.Rank}_of_{card.Suit}.png")),
                Width = 100,
                Height = 150,
                Margin = new Thickness(5)
            };
            DealerHand.Children.Add(cardImage);
        }

        PotAmount.Text = $"Пот: ${player.Bet + dealer.Bet}";

        PlayerStatus.Text = $"Ваш счет: {player.Score}";
        PlayerBalance.Text = $"Баланс игрока: ${player.Balance}";
        DealerStatus.Text = $"Счет дилера: {dealer.Score}";
        DealerBalance.Text = $"Баланс дилера: ${dealer.Balance}";
    }

    private void StartNewGame_Click(object sender, RoutedEventArgs e)
    {
        StartNewGame();
    }
}
