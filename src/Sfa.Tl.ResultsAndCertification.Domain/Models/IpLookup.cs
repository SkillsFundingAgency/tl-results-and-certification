using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class IpLookup : BaseEntity
    {
        public int TlLookupId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ShowOption { get; set; }
        public int? SortOrder { get; set; }

        public virtual TlLookup TlLookup { get; set; }
    }
}
