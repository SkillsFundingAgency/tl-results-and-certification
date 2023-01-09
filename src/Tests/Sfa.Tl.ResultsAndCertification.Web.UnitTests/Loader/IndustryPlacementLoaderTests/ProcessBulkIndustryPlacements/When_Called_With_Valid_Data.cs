using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.ProcessBulkIndustryPlacements
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            BulkIndustryPlacementResponse = new BulkIndustryPlacementResponse
            {
                IsSuccess = true,
                Stats = new BulkUploadStats
                {
                    TotalRecordsCount = 10
                }
            };

            UploadIndustryPlacementsRequestViewModel = new UploadIndustryPlacementsRequestViewModel { AoUkprn = Ukprn, File = FormFile };

            UploadIndustryPlacementsResponseViewModel = new UploadIndustryPlacementsResponseViewModel
            {
                IsSuccess = true,
                Stats = new BulkUploadStatsViewModel
                {
                    TotalRecordsCount = 10
                }
            };

            Mapper.Map<BulkProcessRequest>(UploadIndustryPlacementsRequestViewModel).Returns(BulkIndustryPlacementRequest);
            Mapper.Map<UploadIndustryPlacementsResponseViewModel>(BulkIndustryPlacementResponse).Returns(UploadIndustryPlacementsResponseViewModel);
            InternalApiClient.ProcessBulkIndustryPlacementsAsync(BulkIndustryPlacementRequest).Returns(BulkIndustryPlacementResponse);
            Loader = new IndustryPlacementLoader(InternalApiClient, Mapper, BlobStorageService);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            InternalApiClient.Received(1).ProcessBulkIndustryPlacementsAsync(BulkIndustryPlacementRequest);
            BlobStorageService.Received(1).UploadFileAsync(Arg.Any<BlobStorageData>());
            Mapper.Received().Map<BulkProcessRequest>(UploadIndustryPlacementsRequestViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsSuccess.Should().Be(UploadIndustryPlacementsResponseViewModel.IsSuccess);
            ActualResult.Stats.Should().NotBeNull();

            ActualResult.Stats.TotalRecordsCount.Should().Be(UploadIndustryPlacementsResponseViewModel.Stats.TotalRecordsCount);
        }
    }
}
