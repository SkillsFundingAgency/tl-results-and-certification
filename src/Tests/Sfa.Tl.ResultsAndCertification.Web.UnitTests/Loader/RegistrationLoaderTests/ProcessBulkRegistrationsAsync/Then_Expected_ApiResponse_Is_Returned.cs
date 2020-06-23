using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessBulkRegistrationsAsync
{
    public class Then_Expected_ApiResponse_Is_Returned : When_ProcessBulkRegistrationsAsync_Is_Called
    {
        [Fact]
        public void Then_UploadFileAsync_Is_Called()
        {
            BlobStorageService.Received(1).UploadFileAsync(Arg.Any<BlobStorageData>());
        }

        [Fact]
        public void Then_Expected_UploadRegistrationResponse_Are_Returned()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.IsSuccess.Should().Be(UploadRegistrationsResponseViewModel.IsSuccess);
            ActualResult.ErrorFileSize.Should().Be(UploadRegistrationsResponseViewModel.ErrorFileSize);
            ActualResult.BlobUniqueReference.Should().Be(UploadRegistrationsResponseViewModel.BlobUniqueReference);
        }
    }
}
