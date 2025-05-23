using System;
using System.Collections.Generic;

namespace BlackJackApp.Models
{
    public class Deck
    {
        private readonly List<Card> _cards;
        private readonly Random _random;

        public Deck()
        {
            _random = new Random();
            _cards = CreateDeck();
        }

        private static List<Card> CreateDeck()
        {
            var cards = new List<Card>();
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };
            int[] values = { 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10, 11 };

            foreach (var suit in suits)
            {
                for (int i = 0; i < ranks.Length; i++)
                {
                    cards.Add(new Card(suit, ranks[i], values[i]));
                }
            }
            return cards;
        }

        public void Shuffle()
        {
            int n = _cards.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                (_cards[k], _cards[n]) = (_cards[n], _cards[k]);
            }
        }

        public Card DrawCard()
        {
            if (_cards.Count == 0)
            {
                throw new InvalidOperationException("Колода пуста.");
            }
            Card card = _cards[0];
            _cards.RemoveAt(0);
            return card;
        }

        public int CardsRemaining() => _cards.Count;

        public void ResetAndShuffle()
        {
            _cards.Clear();
            _cards.AddRange(CreateDeck());
            Shuffle();
        }
    }
}