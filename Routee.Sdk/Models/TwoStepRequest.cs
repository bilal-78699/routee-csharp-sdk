using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Two Step Request
    /// </summary>
    public class TwoStepRequest
    {
        public string method { get; set; }
        public string type { get; set; }
        public string recipient { get; set; }
        public Dictionary<string,string> arguments { get; set; }
        public string template { get; set; }
        public string templateCountry { get; set; }
        public string originator { get; set; }
        public decimal? lifetimeInSeconds { get; set; }
        public decimal? maxRetries { get; set; }
        public decimal? digits { get; set; }
    }
}
