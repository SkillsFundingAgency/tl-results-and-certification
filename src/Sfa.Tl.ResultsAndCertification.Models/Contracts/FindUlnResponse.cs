using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class FindUlnResponse
    {
        public int RegistrationProfileId { get; set; }
        public long Uln { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
        public bool IsRegisteredWithOtherAo { get; set; }        
    }
}
