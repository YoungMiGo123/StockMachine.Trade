using System;
using System.Collections.Generic;
using System.Text;

namespace Trading.Core.Models
{
    public class Stock
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Volume { get; set; }
        public double Price { get; set; }
        public string ChangePercent { get; set; }
        public string CompanyOverview { get; set; }
        public string ContanctInfo { get; set; }
        public string PostalAddress { get; set; }
        public string PhysicalAddress { get; set; }
        public string Symbol { get; set; }
        public string Industry { get; set; }
        public string Sector { get; set; }
        public string CompanyRegistrationNumber { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ListingDate { get; set; }
        public string RegulatoryDocuments { get;set;}
        public IEnumerable<StockPeer> SectorPeers { get; set; }
        public string InstrumentUrl { get; set; }
        public string ProfileUrl { get; set; }
    }
}
