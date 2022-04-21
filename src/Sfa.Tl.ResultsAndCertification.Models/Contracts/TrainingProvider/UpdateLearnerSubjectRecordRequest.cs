using Sfa.Tl.ResultsAndCertification.Common.Enum;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider
{
    public class UpdateLearnerSubjectRequest
    {
        public int ProfileId { get; set; }
        public SubjectStatus? SubjectStatus { get; set; }
        public SubjectType SubjectType { get; set; }
        public long ProviderUkprn { get; set; }
        public string PerformedBy { get; set; }
    }
}
