using System;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Sms Response
    /// </summary>
    public class SmsResponse : BaseSms
    {
        public DateTime createdAt { get; set; }
        public bool flash { get; set; }
        public BodyAnalysis bodyAnalysis { get; set; }
        public Callback callback { get; set; }
        public string status { get; set; }
        public string label { get; set; }
        public string trackingId { get; set; }
    }
}