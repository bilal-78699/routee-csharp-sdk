using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Represents basic sms info
    /// </summary>
    public class BaseSms
    {
        public string body { get; set; }
        public string to { get; set; }
        public string from { get; set; }
    }

    /// <summary>
    /// Single Sms
    /// </summary>
    public class SingleSms : BaseSms
    {
        public bool flash { get; set; }
        public string label { get; set; }
        public Callback callback { get; set; }
    }


}
