using System;
using System.Collections.Generic;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// SMS Campaign
    /// </summary>
    public class Campaign
    {
        public Callback callback { get; set; }
        public List<string> contacts { get; set; }
        public Dictionary<string,string> fallbackValues { get; set; }
        public bool flash { get; set; }
        public List<string> groups { get; set; }
        public string body { get; set; }
        public string campaignName { get; set; }
        public List<string> to { get; set; }
        public bool respectQuietHours { get; set; }
        public DateTime scheduledDate { get; set; }
        public string from { get; set; }
        public CampaignCallback campaignCallback { get; set; }
        public Reminder reminder { get; set; }
    }
}