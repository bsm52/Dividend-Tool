using AlphaVantage.Net.Stocks;
using AlphaVantage.Net.Stocks.BatchQuotes;
using AlphaVantage.Net.Stocks.TimeSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dividend_tool
{
    public class StockProvider
    {

        public async Task<List<KeyValuePair<string, double>>> GetStockPrices(List<KeyValuePair<string, int>> stocks)
        {
            string apiKey = "O8UW3NE3Z3CEYSVB"; // API KEY

            var client = new AlphaVantageStocksClient(apiKey);


            //This list will contain the stocks' symbols and the prices
            List<KeyValuePair<string, double>> stocksAndPrice = new List<KeyValuePair<string, double>>();

            string[] stockSymbols = new string[stocks.Count];

            for(int i = 0; i < stocks.Count; i++)
            {
                stockSymbols[i] = stocks[i].Key;
            }


            // retrieve stocks batch quotes for Apple Inc. and Facebook Inc.:
            ICollection<StockQuote> batchQuotes = await client.RequestBatchQuotesAsync(stockSymbols);
            foreach (var stockQuote in batchQuotes)
            {
                stocksAndPrice.Add(new KeyValuePair<string, double>(stockQuote.Symbol, (double)stockQuote.Price));
                Console.WriteLine($"{stockQuote.Symbol}: {stockQuote.Price}");
            }

            return stocksAndPrice;
            
        }


    }
}
