using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement
{
    public class FindSoaLearnerRecord
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string ProviderName { get; set; }
        public string TlevelTitle { get; set; }        
        public RegistrationPathwayStatus Status { get; set; }
        public bool IsLearnerRegistered => Status == RegistrationPathwayStatus.Active || Status == RegistrationPathwayStatus.Withdrawn;

        public bool IsNotWithdrawn => Status == RegistrationPathwayStatus.Active;
    }
}