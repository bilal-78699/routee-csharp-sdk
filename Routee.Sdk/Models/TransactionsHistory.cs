using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Basic fields for search result response
    /// </summary>
    public class SearchBase
    {
        public decimal totalPages { get; set; }
        public decimal totalElements { get; set; }
        public bool last { get; set; }
        public decimal numberOfElements { get; set; }
        public bool first { get; set; }
        public decimal size { get; set; }
        public decimal number { get; set; }
    }

    /// <summary>
    /// Transaction History
    /// </summary>
    public class TransactionsHistory:SearchBase
    {
        public List<TransactionHistoryContent> content { get; set; }
    }

    public class TransactionHistoryContent
    {
        public string id { get; set; }
        public string source { get; set; }
        public string transactionType { get; set; }
        public decimal amount { get; set; }
        public string status { get; set; }
        public decimal balanceBefore { get; set; }
        public decimal balanceAfter { get; set; }
        public DateTime date { get; set; }
        public List<Action> actions { get; set; }
    }
}
