using System.Collections.Generic;

namespace Routee.Sdk.Models
{
    public class AnalyzeCampaign
    {
        public List<string> contacts { get; set; }
        public List<string> groups { get; set; }
        public string body { get; set; }
        public List<string> to { get; set; }
        public string from { get; set; }
    }
}