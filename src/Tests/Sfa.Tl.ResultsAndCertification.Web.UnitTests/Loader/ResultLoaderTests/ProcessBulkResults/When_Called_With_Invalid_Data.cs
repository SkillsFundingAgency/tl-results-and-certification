using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.ProcessBulkResults
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        [Fact]
        public void Then_Expected_Methods_Called()
        {
            BlobStorageService.Received(1).UploadFileAsync(Arg.Any<BlobStorageData>());
        }

        [Fact]
        public void Then_Expected_UploadResultsResponse_Are_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsSuccess.Should().Be(UploadResultsResponseViewModel.IsSuccess);
            ActualResult.ErrorFileSize.Should().Be(UploadResultsResponseViewModel.ErrorFileSize);
            ActualResult.BlobUniqueReference.Should().Be(UploadResultsResponseViewModel.BlobUniqueReference);
        }
    }
}