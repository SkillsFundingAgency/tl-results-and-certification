using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard
{
    public class AdminLearnerRecord
    {
        public int RegistrationPathwayId { get; set; }

        public long Uln { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public DateTime DateofBirth { get; set; }

        public bool IsRegistered
            => Pathway != null && Pathway.Status == RegistrationPathwayStatus.Active || Pathway.Status == RegistrationPathwayStatus.Withdrawn;

        public SubjectStatus? MathsStatus { get; set; }

        public SubjectStatus? EnglishStatus { get; set; }

        public CalculationStatus? OverallCalculationStatus { get; set; }

        public Pathway Pathway { get; set; }

        public AwardingOrganisation AwardingOrganisation { get; set; }
    }
}