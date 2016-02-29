using LuccaDevises.businessLayer;
using LuccaDevises.dataLayer;
using System;

namespace LuccaDevises
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args == null || args.Length<1)
                throw new ArgumentNullException("Exchange rates file path needed"); 

            IDataReader reader = new FileReader(args[0]); 
            ICurrencyConverter converter = new CurrencyConverter();


            var data = reader.GetData();
            Console.WriteLine(converter.ConvertCurrency(data.Item1, data.Item2));
        }
    }
}
