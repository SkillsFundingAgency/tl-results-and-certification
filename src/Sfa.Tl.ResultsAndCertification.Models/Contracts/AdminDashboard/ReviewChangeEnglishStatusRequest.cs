using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;

public class ReviewChangeEnglishStatusRequest : ReviewChangeRequest
{
    public SubjectStatus? EnglishStatusFrom { get; set; }
    public SubjectStatus? EnglishStatusTo { get; set; }
    public override ChangeType ChangeType => ChangeType.EnglishStatus;
}