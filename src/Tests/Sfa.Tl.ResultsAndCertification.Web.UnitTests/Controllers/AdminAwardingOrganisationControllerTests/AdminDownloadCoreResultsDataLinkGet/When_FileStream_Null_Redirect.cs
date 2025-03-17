using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using System.IO;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminDownloadCoreResultsDataLinkGet
{
    public class When_FileStream_Null_Redirect : AdminDownloadCoreResultsDataLinkGetBaseTest
    {
        public override void Given()
        {
            ResultLoader.GetResultsDataFileAsync(Ukprn, FileGuid, ComponentType.Core).Returns(null as Stream);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).GetResultsDataFileAsync(Ukprn, FileGuid, ComponentType.Core);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}