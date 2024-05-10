using Sfa.Tl.ResultsAndCertification.Common.Extensions;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment
{
    public class AdminOccupationalSpecialismViewModel : AdminAssessmentLearnerDetails
    {
        public int SpecialismAssessmentId { get; set; }

        public string SpecialismAssessmentName { get; set; }

        public bool HasReachedAssessmentsThreashold
            => ValidPathwayAssessmentSeries.IsNullOrEmpty();
    }
}