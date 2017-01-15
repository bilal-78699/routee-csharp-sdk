using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Two Step Account HistoryReport
    /// </summary>
    public class TwoStepAccountHistoryReport
    {
        public decimal total { get; set; }
        public Dictionary<string,int> totals { get; set; }
        public Dictionary<string,Dictionary<string,int>> perCountry { get; set; }
    }
}
