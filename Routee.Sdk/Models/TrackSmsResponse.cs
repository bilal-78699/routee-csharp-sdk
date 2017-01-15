using System.Collections.Generic;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Track SMS Response
    /// </summary>
    public class TrackSmsResponse
    {
        public string applicationName { get; set; }
        public string country { get; set; }
        public string smsId { get; set; }
        public string messageId { get; set; }
        public List<string> groups { get; set; }
        public string body { get; set; }
        public string @operator { get; set; }
        public string originatingService { get; set; }
        public string to { get; set; }
        public string label { get; set; }
        public Status status { get; set; }
        public decimal latency { get; set; }
        public decimal parts { get; set; }
        public decimal price { get; set; }
    }
}