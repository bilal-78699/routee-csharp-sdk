using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Quiet Hour Country
    /// </summary>
    public class QuietHourCountry
    {
        public string code { get; set; }
        public string name { get; set; }
        public string localeName { get; set; }
        public bool supported { get; set; }
    }
}
