using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Two Step Response
    /// </summary>
    public class TwoStepResponse
    {
        public string trackingId { get; set; }
        public string status { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
