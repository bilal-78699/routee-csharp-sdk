namespace Routee.Sdk.Models
{
    public class Bank
    {
        public string name { get; set; }
        public string address { get; set; }
        public string number { get; set; }
        public string iban { get; set; }
        public string currency { get; set; }
        public decimal minAmount { get; set; }
        public string country { get; set; }
        public string swiftCode { get; set; }
    }
}