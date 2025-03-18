using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using System.IO;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminDownloadRommsDataLinkAsyncGet
{
    public class When_FileStream_Null_Redirect : AdminDownloadRommsDataLinkAsyncGetBaseTest
    {
        public override void Given()
        {
            PostResultsLoader.GetRommsDataFileAsync(Ukprn, FileGuid).Returns(null as Stream);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            PostResultsLoader.GetRommsDataFileAsync(Ukprn, FileGuid);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}