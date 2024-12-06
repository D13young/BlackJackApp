using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJackApp;

public class Card
{
    public string Suit { get; set; }
    public string Rank { get; set; }
    public int Value { get; set; }
    public string ImagePath { get; set; }

    public Card(string suit, string rank, int value)
    {
        Suit = suit;
        Rank = rank;
        Value = value;
        ImagePath = $"Images/{Rank}_of_{Suit}.png";
    }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}
