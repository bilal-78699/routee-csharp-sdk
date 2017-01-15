using System.Collections.Generic;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// SMS Body Analysis
    /// </summary>
    public class BodyAnalysis
    {
        public decimal characters { get; set; }
        public decimal parts { get; set; }
        public Transcode transcode { get; set; }
        public bool unicode { get; set; }
        public List<string> unsupportedGSMCharacters { get; set; }
    }
}