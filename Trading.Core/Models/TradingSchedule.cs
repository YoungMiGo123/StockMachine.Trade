using System;
using System.Collections.Generic;
using System.Text;

namespace Trading.Core.Models
{
    public class TradingSchedule
    {
        public Guid Id { get; set; }
        public string PreTradingSession { get; set; }
        public string CoreTradingSessions { get; set; }
        public string ExtendedHours { get; set; }
    }
}
