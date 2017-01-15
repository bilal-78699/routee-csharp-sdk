using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Contact List
    /// </summary>
    public class ContactList : SearchBase
    {
        public List<ContactResponse> content { get; set; }
    }
}
