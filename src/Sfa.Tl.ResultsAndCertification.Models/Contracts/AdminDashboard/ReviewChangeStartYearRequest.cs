using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class ReviewChangeStartYearRequest : ReviewChangeRequest
    {
        public ChangeStartYearDetails ChangeStartYearDetails { get; set; }

        public override ChangeType ChangeType => ChangeType.StartYear;
    }
}