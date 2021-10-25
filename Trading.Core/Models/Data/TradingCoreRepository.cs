using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trading.Core.Interfaces;
using Trading.Core.Models.Markets;
using System.Data;

namespace Trading.Core.Models.Data
{
    public class TradingCoreRepository : ITradingCoreRepository
    {
        public TradingCoreRepository(TradingCoreDbContext tradingCoreDbContext)
        {
            TradingCoreDbContext = tradingCoreDbContext;
        }

        public TradingCoreDbContext TradingCoreDbContext { get; }

        public async Task AddCommodities(IEnumerable<Commodities> Commodities)
        {
            await TradingCoreDbContext.Commodities.AddRangeAsync(Commodities);
        }

        public void AddEntity(object Object)
        {
            TradingCoreDbContext.Add(Object);
        }

        public async Task AddStocks(IEnumerable<Stock> stocks)
        {
            await TradingCoreDbContext.Stocks.AddRangeAsync(stocks);
            
        }

        public async Task AddTopMovers(IEnumerable<TopMovers> TopMovers)
        {
            await TradingCoreDbContext.TopMovers.AddRangeAsync(TopMovers);
        }

        public IEnumerable<Stock> GetAllStocks(bool IncludeStockPeers = false)
        {
          
            var StockSummaries = 
                         from s in TradingCoreDbContext.Stocks
                         group s by s.Symbol into StockSummary
                         select new { Symbol = StockSummary.Key, CreatedDateTime = StockSummary.Max(x => x.CreatedDateTime), Id = StockSummary.Max(x => x.Id) };

            var stocks = new List<Stock>();
            if (IncludeStockPeers)
            {
                stocks = TradingCoreDbContext.Stocks.Where(x => StockSummaries.Select(x => x.Id).Contains(x.Id)).Include(x => x.SectorPeers).ToList();
                return stocks;
            }
            stocks = TradingCoreDbContext.Stocks.Where(x => StockSummaries.Select(s => s.Id).Contains(x.Id)).ToList();
            return stocks;
        }
        public Commodities GetCommodities(Guid Id)
        {
            return TradingCoreDbContext.Commodities.FirstOrDefault(x => x.Id == Id);
        }

        public IEnumerable<Commodities> GetCommodities()
        {
            return TradingCoreDbContext.Commodities.ToList();
        }

        public IEnumerable<Industry> GetIndustryPeformance ()
        {
            var industries = new List<Industry>();
            using (var command = TradingCoreDbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "EXEC [pisysuser].[GetIndustryPeformance]";
                command.CommandType = CommandType.Text;
                command.Connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        industries.Add(new Industry()
                        {
                            IndustryName = reader.GetString(0),
                            TotalChangePercent = reader.GetDouble(1),
                            CreatedDateTime = reader.GetDateTime(2)
                        });
                    }
                }
            }
            return industries;
        }

        public Stock GetStock(Guid Id)
        {
            return GetAllStocks().FirstOrDefault(x => x.Id == Id);
        }

        public IEnumerable<StockPeer> GetStockPeers(Guid StockId)
        {
            var StockEntity = GetAllStocks().FirstOrDefault(x => x.Id == StockId);
            if (StockEntity is Stock stock)
            {
                return stock.SectorPeers;
            }
            return new List<StockPeer>();
        }

        public IEnumerable<TopMovers> GetTopMovers()
        {
   
            var movers = TradingCoreDbContext.TopMovers.ToList();
            return movers;
        }

        public IEnumerable<TopMovers> GetTopMoversBy(string type)
        {

            var movers = TradingCoreDbContext.TopMovers.Where(x => x.ChangeFactorType == type);
            return movers;
        }

        public async Task<bool> SaveAllChangesAsync()
        {
            return await TradingCoreDbContext.SaveChangesAsync() > 0;
        }
    }
}
