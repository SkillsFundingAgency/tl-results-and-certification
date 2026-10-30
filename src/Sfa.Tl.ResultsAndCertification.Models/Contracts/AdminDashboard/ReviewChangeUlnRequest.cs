using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ReviewChangeUlnRequest : ReviewChangeRequest
    {
        public ChangeUlnDetails ChangeUlnDetails { get; set; }

        public override ChangeType ChangeType => ChangeType.Uln;
    }
}
