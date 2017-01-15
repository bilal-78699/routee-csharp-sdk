namespace Routee.Sdk.Models
{
    /// <summary>
    /// Recepient Countries
    /// </summary>
    public class RecipientCountries
    {
        public string recipient { get; set; }
        public string recipientCountry { get; set; }
        public bool blacklisted { get; set; }
    }
}