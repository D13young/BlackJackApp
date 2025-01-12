﻿using System;
using System.Collections.Generic;

namespace BlackJackApp.Models;

public class Deck
{
    private readonly List<Card> cards;
    private readonly Random random;

    public Deck()
    {
        random = new Random();
        cards = CreateDeck();
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
        int n = cards.Count;
        for (int i = 0; i < n; i++)
        {
            int j = random.Next(i, n);
            (cards[i], cards[j]) = (cards[j], cards[i]);
        }
    }

    public Card DrawCard()
    {
        if (cards.Count == 0)
        {
            throw new InvalidOperationException("Колода пуста.");
        }
        Card card = cards[0];
        cards.RemoveAt(0);
        return card;
    }

    public int CardsRemaining() => cards.Count;

    public void ResetAndShuffle()
    {
        cards.Clear();
        cards.AddRange(CreateDeck());
        Shuffle();
    }
}