using System.Collections.Generic;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Track SMS Campaign Response
    /// </summary>
    public class TrackSmsCampaignResponse:SearchBase
    {
        public List<TrackSmsResponse> content { get; set; }
    }
}