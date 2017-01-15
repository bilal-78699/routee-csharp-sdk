using System;

namespace Routee.Sdk.Models
{
    public class Action
    {
        public string id { get; set; }
        public string type { get; set; }
        public decimal amount { get; set; }
        public DateTime date { get; set; }
        public decimal balanceBefore { get; set; }
        public decimal balanceAfter { get; set; }
        public string status { get; set; }
    }
}