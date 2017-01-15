using System.Collections.Generic;

namespace Routee.Sdk.Models
{
    /// <summary>
    /// Account Group Name
    /// </summary>
    public class AccountGroupName
    {
        public string name { get; set; }
    }

    /// <summary>
    /// Account Group
    /// </summary>
    public class AccountGroup : AccountGroupName
    {
        public string strategy { get; set; }
        public List<Filter> filters { get; set; }
    }

    /// <summary>
    /// Account Filter
    /// </summary>
    public class Filter
    {
        public string fieldName { get; set; }
        public string searchTerm { get; set; }
    }

    /// <summary>
    /// Account Group Size
    /// </summary>
    public class AccountGroupSize : AccountGroupName
    {
        public int size { get; set; }
    }

    /// <summary>
    /// Deleted Contacts
    /// </summary>
    public class DeletedContacts : AccountGroupName
    {
        public bool deletedContacts { get; set; }
    }

    /// <summary>
    /// Paged Account Group
    /// </summary>
    public class PagedAccountGroup:SearchBase
    {
        public List<AccountGroupSize> content { get; set; }
    }
}