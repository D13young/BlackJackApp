using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackApp;

public class Player
{
    public string Name { get; set; }
    public List<Card> Hand { get; set; }
    public int Score { get; set; }
    public int Balance { get; set; }
    public int Bet { get; set; }

    public Player(string name)
    {
        Name = name;
        Hand = new List<Card>();
        Score = 0;
        Balance = 1000;
        Bet = 0;
    }

    public void PlaceBet(int amount)
    {
        if (amount <= Balance)
        {
            Bet = amount;
            Balance -= amount;
        }
        else
        {
            throw new InvalidOperationException("Недостаточно средств для ставки.");
        }
    }

    public void WinBet()
    {
        Balance += Bet * 2;
        Bet = 0;
    }

    public void LoseBet()
    {
        Bet = 0;
    }
}