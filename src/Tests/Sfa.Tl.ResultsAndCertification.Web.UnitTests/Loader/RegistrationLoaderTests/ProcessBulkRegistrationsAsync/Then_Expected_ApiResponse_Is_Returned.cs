using FluentAssertions;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessBulkRegistrationsAsync
{
    public class Then_Expected_ApiResponse_Is_Returned : When_ProcessBulkRegistrationsAsync_Is_Called
    {
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
