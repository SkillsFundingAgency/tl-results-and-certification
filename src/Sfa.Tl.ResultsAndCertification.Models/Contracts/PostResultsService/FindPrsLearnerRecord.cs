using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService
{
    public class FindPrsLearnerRecord
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
        public long ProviderUkprn { get; set; }
        public string ProviderName { get; set; }
        public string TlevelTitle { get; set; }
        public RegistrationPathwayStatus Status { get; set; }

        public bool IsWithdrawn => Status == RegistrationPathwayStatus.Withdrawn;
    }
}
