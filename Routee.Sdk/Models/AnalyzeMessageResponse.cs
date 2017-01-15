namespace Routee.Sdk.Models
{
    /// <summary>
    /// Analayze Message Response
    /// </summary>
    public class AnalyzeMessageResponse
    {
        public BodyAnalysis bodyAnalysis { get; set; }
        public decimal cost { get; set; }
    }
}