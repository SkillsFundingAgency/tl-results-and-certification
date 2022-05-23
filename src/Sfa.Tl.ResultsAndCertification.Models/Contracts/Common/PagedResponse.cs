using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Common
{
    public class PagedResponse<T>
    {
        public int TotalRecords { get; set; }
        public IList<T> Records { get; set; }
    }
}
