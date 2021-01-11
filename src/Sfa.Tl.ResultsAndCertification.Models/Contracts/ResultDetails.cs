using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts
{
    public class ResultDetails
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public long ProviderUkprn { get; set; }
        public string ProviderName { get; set; }

        public RegistrationPathwayStatus Status { get; set; }
    }
}
