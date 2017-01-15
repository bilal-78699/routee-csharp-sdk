using System;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Label
    /// </summary>
    public class Label
    {
        public string name { get; set; }
        /// <summary>
        /// It can be string or int depending upon type of label
        /// </summary>
        public object value { get; set; }
    }
}