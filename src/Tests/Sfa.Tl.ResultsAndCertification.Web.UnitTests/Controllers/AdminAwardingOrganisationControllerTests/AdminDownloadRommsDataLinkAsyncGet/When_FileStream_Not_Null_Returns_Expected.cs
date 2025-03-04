using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminAwardingOrganisation;
using Sfa.Tl.ResultsAndCertification.Web.FileResult;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminAwardingOrganisationControllerTests.AdminDownloadRommsDataLinkAsyncGet
{
    public class When_FileStream_Not_Null_Returns_Expected : AdminDownloadRommsDataLinkAsyncGetBaseTest
    {
        public override void Given()
        {
            PostResultsLoader.GetRommsDataFileAsync(Ukprn, FileGuid).Returns(Stream);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            PostResultsLoader.GetRommsDataFileAsync(Ukprn, FileGuid);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType<CsvFileStreamResult>();

            CsvFileStreamResult viewResult = Result as CsvFileStreamResult;
            viewResult.FileDownloadName.Should().Be(AdminDownloadResultsRommsByAwardingOrganisation.Romms_Data_Report_File_Name_Text);
            viewResult.FileStream.Should().BeSameAs(Stream);
        }
    }
}