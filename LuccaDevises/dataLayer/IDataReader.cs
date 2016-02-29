using System;
using System.Collections.Generic;

namespace LuccaDevises.dataLayer
{
    public interface IDataReader
    {
        Tuple<BrainTeaser, List<ExchangeRate>> GetData();
    }
}
