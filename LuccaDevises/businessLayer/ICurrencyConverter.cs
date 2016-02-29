using LuccaDevises.dataLayer;
using System.Collections.Generic;

namespace LuccaDevises.businessLayer
{
    public interface ICurrencyConverter
    {
        int ConvertCurrency(BrainTeaser problem, List<ExchangeRate> rates );
    }
}
