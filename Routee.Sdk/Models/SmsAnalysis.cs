
using System.Collections.Generic;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// SMS Analysis
    /// </summary>
    public class SmsAnalysis
    {
        public BodyAnalysis bodyAnalysis { get; set; }
        public Dictionary<string,RecipientCountries> contacts { get; set; }
        public decimal numberOfRecipients { get; set; }
        public Dictionary<string,string> recipientCountries { get; set; }
        public Dictionary<string,int> recipientsPerCountry { get; set; }
        public Dictionary<string,int> recipientsPerGroup { get; set; }
        public decimal totalInGroups { get; set; }
    }
}