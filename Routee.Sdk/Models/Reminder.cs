using System.Collections.Generic;

namespace Routee.Sdk.Models
{
    public class Reminder
    {
        public decimal minutesAfter { get; set; }
        public decimal minutesBefore { get; set; }
        public List<string> to { get; set; }
    }
}