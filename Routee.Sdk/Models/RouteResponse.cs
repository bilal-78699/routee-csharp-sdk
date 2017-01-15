using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Route response
    /// </summary>
    /// <typeparam name="T">Type of response</typeparam>
    public class RouteResponse<T>
    {
        /// <summary>
        /// Status code received
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Response recieved, will be null if there are errors
        /// </summary>
        public T Response { get; set; }

        /// <summary>
        /// Indicates response has a value
        /// </summary>
        public bool HasValue { get { return Response != null; } }

        /// <summary>
        /// Error object
        /// </summary>
        public Error Error { get; set; }
    }
}
