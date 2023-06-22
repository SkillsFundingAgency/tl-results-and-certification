using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Models.OverallResults
{
    public class OverallSpecialismResultDetail
    {
        public List<OverallSpecialismDetail> SpecialismDetails { get; set; }

        public int? TlLookupId { get; set; }

        public string OverallSpecialismResult { get; set; }
    }
}