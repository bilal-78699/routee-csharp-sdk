using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Route service price
    /// </summary>
    public class RouteeServicesPrice
    {
        public List<Sm> sms { get; set; }
        public Lookup lookup { get; set; }
        public TwoStep twoStep { get; set; }
        public Currency currency { get; set; }
    }
}
