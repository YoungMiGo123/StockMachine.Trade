using System;
using System.Collections.Generic;
using System.Text;

namespace Trading.Core.Models
{
    public class StockPeer
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ChangePercent { get; set; }
        public double Change { get; set; }

    }
}
