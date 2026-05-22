using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL
{
    public class StockBalanceErrors
    {
        public StockBalanceErrors()
        {
        }

        [Key, Column(Order = 0)]
        public string OrganizationId { get; set; }

        [Key, Column(Order = 1, TypeName = "datetime2")]
        public DateTime Occurred { get; set; }

        [Key, Column(Order = 2)]
        public string AggregateId { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string ProductMainSegment { get; set; }
        public string ProductSegment { get; set; }
        public string ProductTitle { get; set; }
        public double StockBalance { get; set; }
        public int ActId { get; set; }
        public string ActDescription { get; set; }
        public DateTime? ActOccurred { get; set; }

    }
}