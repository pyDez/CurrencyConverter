namespace LuccaDevises.dataLayer
{
    public class BrainTeaser
    {
        public BrainTeaser(string leftCurrency, string rightCurrency, double amount)
        {
            this.LeftCurrency = leftCurrency;
            this.RightCurrency = rightCurrency;
            this.Amount = amount;
        }
        public string LeftCurrency { get; private set; }
        public string RightCurrency { get; private set; }
        public double Amount { get; private set; }
    }
}
