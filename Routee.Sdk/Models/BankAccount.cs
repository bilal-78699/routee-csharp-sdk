using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Bank Account
    /// </summary>
    public class BankAccount
    {
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string vatId { get; set; }
        public string email { get; set; }
        public List<Bank> banks { get; set; }
    }
}
