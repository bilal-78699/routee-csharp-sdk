using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Report Result
    /// </summary>
    public class ReportResult
    {
        public string country { get; set; }
        public string @operator { get; set; }
        public string mcc { get; set; }
        public string mnc { get; set; }
        public DateTime startDateTime { get; set; }
        public string timeGrouping { get; set; }
        public string smsCampaignId { get; set; }
        public int count { get; set; }
        public int deliveredCount { get; set; }
        public int failedCount { get; set; }
        public int queuedCount { get; set; }
        public int sentCount { get; set; }
        public int undeliveredCount { get; set; }
        public decimal price { get; set; }
    }
}
