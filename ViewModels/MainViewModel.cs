using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BlackJackApp.Models;

namespace BlackJackApp.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly Player player;
    private readonly Dealer dealer;
    private readonly Deck deck;

    private string playerStatus;
    private string dealerStatus;
    private string potAmount;
    private string playerBalance;
    private string dealerBalance;
    private bool canHit;
    private bool canStand;

    public ObservableCollection<Card> PlayerHand { get; }
    public ObservableCollection<Card> DealerHand { get; }

    public string PlayerStatus
    {
        get => playerStatus;
        set => SetField(ref playerStatus, value);
    }

    public string DealerStatus
    {
        get => dealerStatus;
        set => SetField(ref dealerStatus, value);
    }

    public string PotAmount
    {
        get => potAmount;
        set => SetField(ref potAmount, value);
    }

    public string PlayerBalance
    {
        get => playerBalance;
        set => SetField(ref playerBalance, value);
    }

    public string DealerBalance
    {
        get => dealerBalance;
        set => SetField(ref dealerBalance, value);
    }

    public bool CanHit
    {
        get => canHit;
        set => SetField(ref canHit, value);
    }

    public bool CanStand
    {
        get => canStand;
        set => SetField(ref canStand, value);
    }

    public ICommand StartNewGameCommand { get; }
    public ICommand HitCommand { get; }
    public ICommand StandCommand { get; }

    public MainViewModel()
    {
        player = new Player("Игрок");
        dealer = new Dealer("Дилер");
        deck = new Deck();
        deck.Shuffle();

        PlayerHand = new ObservableCollection<Card>();
        DealerHand = new ObservableCollection<Card>();

        StartNewGameCommand = new RelayCommand(StartNewGame);
        HitCommand = new RelayCommand(Hit, () => CanHit);
        StandCommand = new RelayCommand(Stand, () => CanStand);
        CanHit = true;
        CanStand = true;
    }

    private void StartNewGame()
    {
        PlayerHand.Clear();
        DealerHand.Clear();
        ResetGame();
        if (!HasInsufficientBalance())
        {
            PlaceBets(100);
            DealCards();
            UpdateUI();
        }
    }

    private void ResetGame()
    {
        PlayerHand.Clear();
        DealerHand.Clear();
        player.Score = dealer.Score = 0;
        CanHit = true;
        CanStand = true;
    }

    private bool HasInsufficientBalance()
    {
        if (player.Balance <= 0 || dealer.Balance <= 0)
        {
            UpdateUIWithStatus("Игра окончена! У одного из игроков закончились средства.");
            return true;
        }
        return false;
    }

    private void PlaceBets(int amount)
    {
        if (player.Balance < amount || dealer.Balance < amount)
        {
            UpdateUIWithStatus("Недостаточно средств для размещения ставки.");
            return;
        }

        try
        {
            player.PlaceBet(amount);
            dealer.PlaceBet(amount);
        }
        catch (InvalidOperationException ex)
        {
            UpdateUIWithStatus(ex.Message);
        }
    }

    private void DealCards()
    {
        DealerHand.Add(DrawCard());

        for (int i = 0; i < 2; i++)
        {
            PlayerHand.Add(DrawCard());
        }
        UpdateScores();
        UpdateUI();
    }

    private Card DrawCard()
    {
        if (deck.CardsRemaining() == 0)
        {
            deck.ResetAndShuffle();
        }
        return deck.DrawCard();
    }

    private void UpdateScores()
    {
        player.Score = CalculateScore(PlayerHand);
        dealer.Score = CalculateScore(DealerHand);
    }

    private static int CalculateScore(IEnumerable<Card> hand)
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

    private void Hit()
    {
        if (player.Score <= 21 && CanHit)
        {
            PlayerHand.Add(DrawCard());
            UpdateScores();
            UpdateUI();

            if (player.Score > 21)
            {
                UpdateUIWithStatus("Игрок проиграл! Перебор!");
                dealer.Balance += player.Bet + dealer.Bet;

                CanHit = false;
                CanStand = false;
            }
        }
    }

    private void Stand()
    {
        CanHit = false;
        CanStand = false;
        while (dealer.Score < 17)
        {
            DealerHand.Add(DrawCard());
            UpdateScores();
            UpdateUI();
        }

        DetermineWinner();
    }

    private void DetermineWinner()
    {
        if (dealer.Score > 21 || player.Score > dealer.Score)
        {
            UpdateUIWithStatus("Игрок выиграл!");
            player.Balance += player.Bet + dealer.Bet;
        }
        else if (player.Score < dealer.Score)
        {
            UpdateUIWithStatus("Дилер выиграл!");
            dealer.Balance += player.Bet + dealer.Bet;
        }
        else if (player.Score == dealer.Score)
        {
            UpdateUIWithStatus("Ничья!");
            player.Balance += player.Bet;
            dealer.Balance += dealer.Bet;
        }
    }

    private void UpdateUI()
    {
        PlayerBalance = $"Баланс игрока: {player.Balance.ToString()}";
        DealerBalance = $"Баланс дилера: {dealer.Balance.ToString()}";
        PotAmount = $"Пот: {player.Bet + dealer.Bet}";
        PlayerStatus = $"Очки игрока: {player.Score}";
        DealerStatus = $"Очки дилера: {dealer.Score}";
    }

    private void UpdateUIWithStatus(string status)
    {
        PlayerStatus = status;
        DealerStatus = string.Empty;
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