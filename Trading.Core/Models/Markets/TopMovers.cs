using System;
using System.Collections.Generic;
using System.Text;

namespace Trading.Core.Models
{
    public class TopMovers
    {
        public Guid Id { get; set; }
        public string Symbol { get; set; }
        public double Change { get; set; }
        public double ChangeFactor { get; set; }
        public string ChangeFactorType { get; set; }
        public string Url { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
