using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Contact Response
    /// </summary>
    public class ContactResponse
    {
        public List<string> blacklistedServices { get; set; }
        public string country { get; set; }
        public List<ResponseLabel> labels { get; set; }
        public string email { get; set; }
        public string firstName { get; set; }
        public string id { get; set; }
        public string lastName { get; set; }
        public List<string> groups { get; set; }
        public string mobile { get; set; }
        public bool vip { get; set; }
    }
}
