using System.Data.SqlClient;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Callback
    /// </summary>
    public class Callback
    {
        public string strategy { get; set; }
        public string url { get; set; }
    }

    /// <summary>
    /// CallBack Strategy
    /// </summary>
    public static class CallbackStrategy
    {
        public const string OnChange = "OnChange";
        public const string OnCompletion = "OnCompletion";
    }
}