using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routee.Sdk.Models
{
    public class AnalyzeCampaignResponse
    {
        public BodyAnalysis bodyAnalysis { get; set; }
        public Dictionary<string,StringRoute> contacts { get; set; }
        public decimal numberOfRecipients { get; set; }
        public Dictionary<string,string> recipientCountries { get; set; }
        public Dictionary<string,int> recipientsPerCountry { get; set; }
        public Dictionary<string,int> recipientsPerGroup { get; set; }
        public decimal totalInGroups { get; set; }
    }
}
