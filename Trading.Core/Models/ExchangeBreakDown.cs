using System;
using System.Collections.Generic;
using System.Text;

namespace Trading.Core.Models
{
    public class ExchangeBreakDown
    {
        public Guid Id { get; set; }
        public string SubSidiaryMarkets { get; set; }
        public string ExchangeTimeZonesInfo { get; set; }
        public string MarketCapitilization { get; set; }
        public string ExchangeCurrency { get; set; }
       

    }
}
