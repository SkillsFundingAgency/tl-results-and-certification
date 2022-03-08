using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService
{
    public class FindPrsLearnerRecord
    {
        public FindPrsLearnerRecord()
        {
            PathwayAssessments = new List<PrsAssessment>();
            SpecialismAssessments = new List<PrsAssessment>();
        }

        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
        public long ProviderUkprn { get; set; }
        public string ProviderName { get; set; }
        public string TlevelTitle { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
        public IEnumerable<PrsAssessment> PathwayAssessments { get; set; }
        public IEnumerable<PrsAssessment> SpecialismAssessments { get; set; }

        public bool IsWithdrawn => Status == RegistrationPathwayStatus.Withdrawn;

        public bool HasResults => PathwayAssessments.Any(pa => pa.HasResult) || SpecialismAssessments.Any(sa => sa.HasResult);
    }
}
