using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace LuccaDevises.dataLayer
{
    public class FileReader : IDataReader
    {
        public FileReader(string filePath)
        {
            this.FilePath = filePath;
        }
        private string FilePath { get; set; }
        public  Tuple<BrainTeaser, List<ExchangeRate>> GetData()
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                throw new NullReferenceException("file path is not set");
            Tuple<BrainTeaser, List<ExchangeRate>> result = new Tuple<BrainTeaser, List<ExchangeRate>>( null, new List<ExchangeRate>()); ;
            int counter = 0;
            int rateNumber = 0;
            string line;
            
             
            // Read the file and display it line by line.
            using (StreamReader reader = new StreamReader(FilePath))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (counter == 0)
                        result = new Tuple<BrainTeaser, List<ExchangeRate>>(parseBrainTeaser(line), new List<ExchangeRate>()) ;
                    if (counter == 1)
                    {
                        if(!int.TryParse(line, out rateNumber))
                            throw new ArgumentException("File is not well formated");
                    }
                    if (counter > 1 && counter < rateNumber+2) //avoid blank line at the end
                        result.Item2.Add(parseExchangeRate(line));
                    counter++;
                }
            }

            return result;
        }

        private BrainTeaser parseBrainTeaser(string stringifiedBrainTeaser)
        {
            var splittedBrainTeaser = stringifiedBrainTeaser.Split(';');
            double amount;
            if (splittedBrainTeaser.Length == 3 && Double.TryParse(splittedBrainTeaser[1], out amount))
                return new BrainTeaser(splittedBrainTeaser[0], splittedBrainTeaser[2], amount);
            throw new ArgumentException("File is not well formated");
        }

        private ExchangeRate parseExchangeRate(string stringifiedExchangeRate)
        {
            var splittedExchangeRate = stringifiedExchangeRate.Split(';');
            double rate;
            if (splittedExchangeRate.Length == 3 && Double.TryParse(splittedExchangeRate[2], NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out rate)) //Parse value using en-US culture
                return new ExchangeRate(splittedExchangeRate[0], splittedExchangeRate[1], rate);
            throw new ArgumentException("File is not well formated");
        }

    }
}
