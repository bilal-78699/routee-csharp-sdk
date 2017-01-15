using System.Collections.Generic;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Sm
    /// </summary>
    public class Sm
    {
        public string mcc { get; set; }
        public string country { get; set; }
        public string iso { get; set; }
        public List<Network> networks { get; set; }
    }
}