using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminDownloadRommsDataLinkAsyncGet
{
    public class When_Id_Not_Guid_Redirect : AdminDownloadRommsDataLinkAsyncGetBaseTest
    {
        public override async Task When()
        {
            Result = await Controller.AdminDownloadRommsDataLinkAsync(Ukprn, "not-a-guid");
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectToProblemWithService();
        }
    }
}