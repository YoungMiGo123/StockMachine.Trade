using System;
using System.Collections.Generic;
using System.Text;

namespace Trading.Core.Models
{
    public class Commodities
    {
        public Guid Id { get; set; }
        public double Price { get; set; }
        
        public string ChangePercent { get; set; }
        public string Volume { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public DateTime CreatedDateTime { get; internal set; }
        public string Name { get; internal set; }
    }
}
