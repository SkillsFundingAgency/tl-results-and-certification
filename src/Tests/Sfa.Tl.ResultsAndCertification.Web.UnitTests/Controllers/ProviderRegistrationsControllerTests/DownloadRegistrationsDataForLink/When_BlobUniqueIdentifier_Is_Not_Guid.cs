using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ProviderRegistrationsControllerTests.DownloadRegistrationsDataForLink
{
    public class When_BlobUniqueIdentifier_Is_Not_Guid : TestSetup
    {
        public override void Given()
        {
        }

        public override async Task When()
        {
            await When("not-a-guid");
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.ShouldBeRedirectToRouteResult(RouteConstants.Error, ("StatusCode", 500));
        }
    }
}