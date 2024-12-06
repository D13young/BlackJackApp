using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackApp;

public class Deck
{
    private List<Card> cards;
    private readonly Random random;

    public Deck()
    {
        random = new Random();
        CreateDeck();
    }

    private void CreateDeck()
    {
        cards = new List<Card>();
        string[] suits = { "hearts", "diamonds", "clubs", "spades" };
        string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };
        int[] values = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11 }; // Значения карт

        foreach (var suit in suits)
        {
            for (int i = 0; i < ranks.Length; i++)
            {
                cards.Add(new Card(suit, ranks[i], values[i]));
            }
        }
    }

    public void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int j = random.Next(i, cards.Count);
            var temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }

    public Card DrawCard()
    {
        if (cards.Count == 0)
        {
            throw new InvalidOperationException("Deck is empty.");
        }
        Card card = cards[0];
        cards.RemoveAt(0);
        return card;
    }

    public int CardsRemaining()
    {
        return cards.Count;
    }

    public void ResetAndShuffle()
    {
        CreateDeck();
        Shuffle();
    }
}