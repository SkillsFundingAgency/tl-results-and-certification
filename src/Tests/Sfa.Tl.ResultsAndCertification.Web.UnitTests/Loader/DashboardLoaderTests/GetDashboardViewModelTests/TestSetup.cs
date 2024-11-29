using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Dashboard;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DashboardLoaderTests.GetDashboardViewModelTests
{
    public abstract class TestSetup : DashboardLoaderBaseTest
    {
        protected ClaimsPrincipal ClaimsPrincipal; 
        protected DashboardViewModel ActualResult;

        public async override Task When()
        {
            ActualResult = await Loader.GetDashboardViewModel(ClaimsPrincipal);
        }
    }
}
