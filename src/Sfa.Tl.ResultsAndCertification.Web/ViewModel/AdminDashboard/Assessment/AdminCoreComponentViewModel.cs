using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment
{
    public class AdminCoreComponentViewModel : AdminAssessmentLearnerDetails
    {
        public bool HasCoreAssessmentEntries { get; set; }

        public bool HasReachedAssessmentsThreashold
            => AssessmentDetails?.PathwayAssessments?.Count() == Constants.AdminAssessmentEntryLimit
            || ValidPathwayAssessmentSeries.IsNullOrEmpty();
    }
}
