using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminAwardingOrganisation;
using Sfa.Tl.ResultsAndCertification.Web.FileResult;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminDownloadSpecialismResultsDataLinkGet
{
    public class When_FileStream_Not_Null_Returns_Expected : AdminDownloadSpecialismResultsDataLinkGetBaseTest
    {
        public override void Given()
        {
            ResultLoader.GetResultsDataFileAsync(Ukprn, FileGuid, ComponentType.Specialism).Returns(Stream);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).GetResultsDataFileAsync(Ukprn, FileGuid, ComponentType.Specialism);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType<CsvFileStreamResult>();

            CsvFileStreamResult viewResult = Result as CsvFileStreamResult;
            viewResult.FileDownloadName.Should().Be(AdminDownloadResultsRommsByAwardingOrganisation.Specialism_Results_Download_FileName);
            viewResult.FileStream.Should().BeSameAs(Stream);
        }
    }
}