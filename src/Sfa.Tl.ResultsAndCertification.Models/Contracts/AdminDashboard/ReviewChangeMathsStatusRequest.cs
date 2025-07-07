using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;

public class ReviewChangeMathsStatusRequest : ReviewChangeRequest
{
    public SubjectStatus? MathsStatusTo { get; set; }
    public override ChangeType ChangeType => ChangeType.MathsStatus;
}