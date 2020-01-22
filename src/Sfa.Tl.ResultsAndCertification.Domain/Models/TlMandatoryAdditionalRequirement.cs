using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class TlMandatoryAdditionalRequirement : BaseEntity
    {
        public string Name { get; set; }
        public bool IsRegulatedQualification { get; set; }
        public string LarId { get; set; }
    }
}
