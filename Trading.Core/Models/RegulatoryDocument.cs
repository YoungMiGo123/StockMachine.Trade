using System;
using System.Collections.Generic;
using System.Text;

namespace Trading.Core.Models
{
    public class RegulatoryDocument
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CraetedDateTime { get; set; }
        public string ListedDate { get; set; }
        public string Url { get; set; }
        public string Year { get; set; }
    }
}
