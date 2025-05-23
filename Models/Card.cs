using System;
using System.Windows.Media.Imaging;

namespace BlackJackApp.Models
{
    public class Card
    {
        public string Suit { get; }
        public string Rank { get; }
        public int Value { get; }
        public BitmapImage? Image { get; }

        public Card(string suit, string rank, int value)
        {
            Suit = suit;
            Rank = rank;
            Value = value;

            try
            {
                Image = new BitmapImage(new Uri($"pack://application:,,,/Images/{rank}_of_{suit}.gif"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                Image = null;
            }
        }
    }
}