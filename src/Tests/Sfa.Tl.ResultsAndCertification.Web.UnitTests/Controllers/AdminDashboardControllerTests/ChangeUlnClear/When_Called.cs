using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ChangeUlnClear
{
    public class When_Called : TestSetup
    {
        public override void Given()
        {

        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            await CacheService.Received(1).RemoveAsync<AdminChangeUlnViewModel>(CacheKey);
        }
    }
}
