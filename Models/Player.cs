using System;

namespace BlackJackApp.Models
{
    public class Player
    {
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public int Balance { get; set; }
        public int Bet { get; set; }
        public int Score { get; set; }

        public Player(string name)
        {
            Name = name;
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
    }
}