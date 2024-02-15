using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using PathwayAssessments = Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment
{
    public class AdminCoreComponentViewModel : AdminAssessmentLearnerDetails
    {
        public AdminAssessmentDetailsViewModel AssessmentDetails { get; set; }

        public IList<PathwayAssessments> ValidPathwayAssessmentSeries { get; set; }

        public bool IsLearnerRegisteredFourYearsAgo => DateTime.Now.Year - StartYear > 4;

        public bool HasCoreAssessmentEntries { get; set; }

        public bool HasReachedAssessmentsThreashold => AssessmentDetails?.PathwayAssessments?.Count() == Constants.AdminAssessmentEntryLimit && !ValidPathwayAssessmentSeries.Any();
    }
}
