using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using PathwayAssessments = Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment
{
    public class AdminOccupationalSpecialismViewModel : AdminAssessmentLearnerDetails
    {
        public int SpecialismAssessmentId { get; set; }

        public AdminAssessmentDetailsViewModel AssessmentDetails { get; set; }

        public IEnumerable<PathwayAssessments> ValidPathwayAssessmentSeries { get; set; }

        public bool IsLearnerRegisteredFourYearsAgo => DateTime.Now.Year - StartYear > 4;

        public bool HasReachedAssessmentsThreashold => ValidPathwayAssessmentSeries != null && !ValidPathwayAssessmentSeries.Any();
    }
}
