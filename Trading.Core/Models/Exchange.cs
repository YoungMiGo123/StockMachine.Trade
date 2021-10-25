using System;
using System.Collections.Generic;
using System.Text;

namespace Trading.Core.Models
{
    public class Exchange
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string Market { get; set; }
        public string Overview { get; set; }
        public string ExchangeName { get; set; }
        public string ExchangeSymbol { get; set; }
        public string MarketCap { get; set; }
        public string Hours { get; set; }
        public DateTime CreatedDate { get; set; }
        public TradingSchedule TradingSchedule { get; set; }
        public ExchangeBreakDown ExchangeBreakDown { get; set; }
        public string History { get; set; }
        public string TimeZone { get; set; }
        public string MicCode { get; set; }
        public ContactInfo contactInfo { get; set; }
    }
}
