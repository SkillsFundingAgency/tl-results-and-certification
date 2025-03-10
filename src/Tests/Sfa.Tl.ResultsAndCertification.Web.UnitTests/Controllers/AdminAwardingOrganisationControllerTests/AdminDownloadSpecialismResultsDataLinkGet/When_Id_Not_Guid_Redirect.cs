using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminDownloadSpecialismResultsDataLinkGet
{
    public class When_Id_Not_Guid_Redirect : AdminDownloadSpecialismResultsDataLinkGetBaseTest
    {
        public override async Task When()
        {
            Result = await Controller.AdminDownloadSpecialismResultsDataLinkAsync(Ukprn, "not-a-guid");
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectToProblemWithService();
        }
    }
}