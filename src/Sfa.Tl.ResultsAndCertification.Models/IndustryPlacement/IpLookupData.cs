using System;

namespace Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement
{
    public class IpLookupData
    {
        // TODO: Change namepsace
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? ShowOption { get; set; }
    }
}
