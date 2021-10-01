using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.Common
{
    public class LoggedInUserTypeInfo
    {
        public long Ukprn { get; set; }
        public string Name { get; set; }
        public LoginUserType UserType { get; set; }
    }
}