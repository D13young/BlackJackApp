namespace BlackJackApp.Models
{
    public class Dealer : Player
    {
        public Dealer(string name) : base(name)
        {
            Balance = 1000;
        }
    }
}