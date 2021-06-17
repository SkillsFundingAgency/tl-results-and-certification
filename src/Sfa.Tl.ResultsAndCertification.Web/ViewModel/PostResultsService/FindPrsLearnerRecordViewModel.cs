using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class FindPrsLearnerRecordViewModel
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public string TlevelTitle { get; set; }
        public RegistrationPathwayStatus Status { get; set; }

        public bool IsValid => Status == RegistrationPathwayStatus.Active;
        public bool IsWithdrawn => Status == RegistrationPathwayStatus.Withdrawn;
    }
}