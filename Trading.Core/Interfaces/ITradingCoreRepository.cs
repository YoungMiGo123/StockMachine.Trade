using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Trading.Core.Models;
using Trading.Core.Models.Markets;

namespace Trading.Core.Interfaces
{
    public interface ITradingCoreRepository
    {
        public IEnumerable<Stock> GetAllStocks(bool IncludeStockPeers = false);
        public IEnumerable<StockPeer> GetStockPeers(Guid Stock);
        public Stock GetStock(Guid Id);
        public Commodities GetCommodities(Guid Id);
        public IEnumerable<Commodities> GetCommodities();
        public IEnumerable<TopMovers> GetTopMovers();
        public IEnumerable<TopMovers> GetTopMoversBy(string type);
        public void AddEntity(object Object);
        public Task AddStocks(IEnumerable<Stock> stocks);
        public Task AddCommodities(IEnumerable<Commodities> Commodities);
        public Task AddTopMovers(IEnumerable<TopMovers> TopMovers);
        public IEnumerable<Industry> GetIndustryPeformance ();
        public Task<bool> SaveAllChangesAsync();
       
    }
}
