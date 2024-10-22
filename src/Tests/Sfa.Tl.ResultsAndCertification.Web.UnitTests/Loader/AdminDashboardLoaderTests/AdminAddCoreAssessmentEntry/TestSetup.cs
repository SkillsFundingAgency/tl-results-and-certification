using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminDashboardLoaderTests.AdminAddCoreAssessmentEntry
{
    public abstract class TestSetup : AdminDashboardLoaderTestsBase
    {
        protected AdminReviewChangesCoreAssessmentViewModel ViewModel; 
        protected bool ActualResult;

        public async override Task When()
        {
            ActualResult = await Loader.ProcessAddCoreAssessmentRequestAsync(ViewModel);
        }
    }
}
