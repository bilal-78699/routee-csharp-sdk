using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Error
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Error Code received
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// Friendly developers message
        /// </summary>
        public string developerMessage { get; set; }

        /// <summary>
        /// Error in the properties sent
        /// </summary>
        public Dictionary<string, string> properties { get; set; }

        /// <summary>
        /// Check if there are any properties
        /// </summary>
        public bool HasProperties
        {
            get { return properties != null && properties.Any(); }
        }
    }
}
