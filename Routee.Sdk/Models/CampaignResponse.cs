using System;
using System.Collections.Generic;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Campaign Response
    /// </summary>
    public class CampaignResponse
    {
        public CampaignCallback campaignCallback { get; set; }
        public List<string> contacts { get; set; }
        public DateTime createdAt { get; set; }
        public Dictionary<string, string> fallbackValues { get; set; }
        public bool flash { get; set; }
        public List<string> groups { get; set; }
        public string body { get; set; }
        public string campaignName { get; set; }
        public List<string> to { get; set; }
        public bool respectQuietHours { get; set; }
        public DateTime scheduledDate { get; set; }
        public string from { get; set; }
        public SmsAnalysis smsAnalysis { get; set; }
        public Callback callback { get; set; }
        public string state { get; set; }
        public Dictionary<string,int> statuses { get; set; }
        public Reminder reminder { get; set; }
        public string trackingId { get; set; }
        public string type { get; set; }
        public Dictionary<string,int> quietHoursReport { get; set; }
    }
}