using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackApp;

public class Dealer : Player
{
    public Dealer(string name) : base(name) { }

    public void Play(Deck deck)
    {
        while (Score < 17)
        {
            Hand.Add(deck.DrawCard());
            Score = CalculateScore(Hand);
        }
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
}
