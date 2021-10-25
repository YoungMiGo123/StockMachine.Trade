using System;
using System.Collections.Generic;
using System.Text;

namespace Trading.Core.Models
{
    public class ContactInfo
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string  Address { get; set; }
        public string MarketComplianceEmail { get; set; }
        public string ClientRelationshipServicesEmail { get; set; }

        public string Website { get; set; }
        public string ContactNumber { get; set; }
        public string Coordinates { get; set; }
    }
}
