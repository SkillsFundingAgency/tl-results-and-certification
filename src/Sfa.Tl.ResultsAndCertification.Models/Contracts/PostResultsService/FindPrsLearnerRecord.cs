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

        public bool IsWithdrawn => Status == RegistrationPathwayStatus.Withdrawn;

        public IEnumerable<PrsAssessment> PathwayAssessments { get; set; }

        public bool IsAssessmentEntryRegistered { get { return PathwayAssessments.Count() > 0; } }
    }

    public class PrsAssessment
    {
        public int AssessmentId { get; set; }
        public string SeriesName { get; set; }
        public bool HasResult { get; set; }
    }
}
