using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.IO;
using System.Text;
using Xunit;
using DownloadOverallResultContent = Sfa.Tl.ResultsAndCertification.Web.Content.DownloadOverallResults.DownloadOverallResults;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DownloadOverallResultsControllerTests.DownloadOverallResultsFile
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            DownloadOverallResultsLoader.DownloadOverallResultsDataAsync(ProviderUkprn, Email)
                .Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File for validation errors")));
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            DownloadOverallResultsLoader.Received(1).DownloadOverallResultsDataAsync(ProviderUkprn, Email);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as FileStreamResult;
            viewResult.Should().NotBeNull();
            viewResult.FileDownloadName.Should().Be(DownloadOverallResultContent.Download_Filename);
            viewResult.ContentType.Should().Be("text/csv");
            viewResult.FileStream.Should().NotBeNull();
        }
    }
}
