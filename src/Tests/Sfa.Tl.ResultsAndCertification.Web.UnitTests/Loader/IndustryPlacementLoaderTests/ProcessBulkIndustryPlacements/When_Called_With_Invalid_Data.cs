using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.IndustryPlacementLoaderTests.ProcessBulkIndustryPlacements
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        [Fact]
        public void Then_Expected_Methods_Called()
        {
            BlobStorageService.Received(1).UploadFileAsync(Arg.Any<BlobStorageData>());
        }

        [Fact]
        public void Then_Expected_UploadAssessmentsResponse_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsSuccess.Should().Be(UploadIndustryPlacementsResponseViewModel.IsSuccess);
            ActualResult.ErrorFileSize.Should().Be(UploadIndustryPlacementsResponseViewModel.ErrorFileSize);
            ActualResult.BlobUniqueReference.Should().Be(UploadIndustryPlacementsResponseViewModel.BlobUniqueReference);
        }
    }
}
