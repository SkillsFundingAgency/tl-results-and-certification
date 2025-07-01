using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;

public class ReviewChangeEnglishStatusRequest : ReviewChangeRequest
{
    public SubjectStatus? EnglishStatusTo { get; set; }
    public override ChangeType ChangeType => ChangeType.SubjectStatus;
}