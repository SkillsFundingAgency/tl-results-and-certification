using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DownloadOverallResultsLoaderTests.DownloadOverallResultsData
{
    public class When_ApiResponse_IsNull : TestSetup
    {
        public override void Given()
        {
            DataExportResponse apiResponse = null;
            InternalApiClient.DownloadOverallResultsDataAsync(providerUkprn, RequestedBy)
                .Returns(apiResponse);
        }


        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).DownloadOverallResultsDataAsync(providerUkprn, RequestedBy);
            BlobStorageService.DidNotReceive().DownloadFileAsync(Arg.Any<BlobStorageData>());
        }

        [Fact]
        public void Then_Returns_Null()
        {
            ActualResult.Should().BeNull();
        }
    }
}
