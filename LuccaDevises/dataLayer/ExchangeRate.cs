using System;

namespace LuccaDevises.dataLayer
{
    public class ExchangeRate
    {
        public ExchangeRate(string leftCurrency, string rightCurrency, double rate)
        {
            this.LeftCurrency = leftCurrency;
            this.RightCurrency = rightCurrency;
            this.Rate = rate; 
        }
        public string LeftCurrency { get; private set; }
        public string RightCurrency { get; private set; }
        public double Rate { get; private set; }
        public double InverseRate
        {
            get { return Math.Round(1 / Rate, 4, MidpointRounding.AwayFromZero); }
        }

    }
}
