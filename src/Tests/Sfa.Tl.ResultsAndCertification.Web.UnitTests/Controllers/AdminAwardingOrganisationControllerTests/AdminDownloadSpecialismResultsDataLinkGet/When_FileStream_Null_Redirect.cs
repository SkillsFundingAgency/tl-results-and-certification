using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using System.IO;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminDownloadSpecialismResultsDataLinkGet
{
    public class When_FileStream_Null_Redirect : AdminDownloadSpecialismResultsDataLinkGetBaseTest
    {
        public override void Given()
        {
            ResultLoader.GetResultsDataFileAsync(Ukprn, FileGuid, ComponentType.Specialism).Returns(null as Stream);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).GetResultsDataFileAsync(Ukprn, FileGuid, ComponentType.Specialism);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}