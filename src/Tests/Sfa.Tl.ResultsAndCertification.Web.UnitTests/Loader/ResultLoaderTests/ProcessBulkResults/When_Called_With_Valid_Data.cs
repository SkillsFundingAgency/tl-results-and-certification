using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.ProcessBulkResults
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            BulkResultResponse = new BulkResultResponse
            {
                IsSuccess = true,
                Stats = new BulkUploadStats
                {
                    TotalRecordsCount = 10
                }
            };

            UploadResultsRequestViewModel = new UploadResultsRequestViewModel { AoUkprn = Ukprn, File = FormFile };

            UploadResultsResponseViewModel = new UploadResultsResponseViewModel
            {
                IsSuccess = true,
                Stats = new BulkUploadStatsViewModel
                {
                    TotalRecordsCount = 10
                }
            };

            Mapper.Map<BulkProcessRequest>(UploadResultsRequestViewModel).Returns(BulkResultRequest);
            Mapper.Map<UploadResultsResponseViewModel>(BulkResultResponse).Returns(UploadResultsResponseViewModel);
            InternalApiClient.ProcessBulkResultsAsync(BulkResultRequest).Returns(BulkResultResponse);
            Loader = new ResultLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).ProcessBulkResultsAsync(BulkResultRequest);
            BlobStorageService.Received(1).UploadFileAsync(Arg.Any<BlobStorageData>());
            Mapper.Received().Map<BulkProcessRequest>(UploadResultsRequestViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsSuccess.Should().Be(UploadResultsResponseViewModel.IsSuccess);
            ActualResult.Stats.Should().NotBeNull();

            ActualResult.Stats.TotalRecordsCount.Should().Be(UploadResultsResponseViewModel.Stats.TotalRecordsCount);
        }
    }
}
