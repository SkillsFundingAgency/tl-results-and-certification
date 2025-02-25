using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDownloadLearnerResults;
using Sfa.Tl.ResultsAndCertification.Web.FileResult;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDownloadLearnerResultsControllerTests;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DownloadOverallResultsControllerTests.AdminDownloadLearnerResultsCsvGet
{
    public class When_File_Generated : AdminDownloadLearnerResultsControllerBaseTest
    {
        private const long ProviderUkprn = 10000536;
        private const string Email = "test@email.com";

        private readonly Stream _stream = new MemoryStream(new byte[] { 1, 2, 3 });
        private IActionResult _result;

        public override void Given()
        {
            DownloadOverallResultsLoader
                .DownloadOverallResultsDataAsync(ProviderUkprn, Email)
                .Returns(_stream);
        }

        public override async Task When()
        {
            _result = await Controller.AdminDownloadLearnerResultsCsvAsync(ProviderUkprn);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            DownloadOverallResultsLoader.Received(1).DownloadOverallResultsDataAsync(ProviderUkprn, Email);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<CsvFileStreamResult>();

            CsvFileStreamResult viewResult = _result as CsvFileStreamResult;
            viewResult.FileDownloadName.Should().Be(AdminDownloadLearnerResultsByProvider.Download_Filename);
            viewResult.FileStream.Should().BeSameAs(_stream);
        }
    }
}