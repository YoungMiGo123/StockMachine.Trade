using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Trading.Core.Models;
using Trading.Core.Models.Data;

namespace StockMachine.Trade.Data.Entity
{
    public static class DbInitializer
    {
        public static void Initialize(TradingCoreDbContext appContext)
        {
            appContext.Database.EnsureCreated();

            // Look for any students.
            if (appContext.Stocks.Any())
            {
                return;   // DB has been seeded
            }
            var filepath = @$"C:\Users\marti\source\repos\StockMachine.Trade\StockMachine.Trade\wwwroot\AllStocks_Withsublinks.json";
            var stocks = GetStocksFromJson(filepath);
            appContext.Stocks.AddRange(stocks);
            appContext.SaveChanges();
        }
        private static List<Stock> GetStocksFromJson(string filepath)
        {
            List<Stock> ListOfStocks = JsonConvert.DeserializeObject<List<Stock>>(File.ReadAllText(filepath));
            return ListOfStocks;
        }
    }
}
