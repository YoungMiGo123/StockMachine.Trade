using System;
using System.Collections.Generic;
using System.Text;

namespace Trading.Core.Models.Markets
{
    public class Industry
    {
        public string IndustryName { get; set; }
        public double TotalChangePercent { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
