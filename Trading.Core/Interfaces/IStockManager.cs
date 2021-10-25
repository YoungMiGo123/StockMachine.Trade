using System.Collections.Generic;
using Trading.Core.Models;

namespace Trading.Core.Interfaces
{
    public interface IStockManager
    {
        public string Url { get; set; }
        public List<Stock> GetAllAvailableStocks();
        public List<Commodities> GetCommodities();
        public Dictionary<string, List<TopMovers>> GetAllTopMoversMetrics();

    }
}
