using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DocumentLoaderTests.GetBulkUploadRegistrationsTechSpecFileAsync
{
    public class Then_Expected_Results_Are_Returned : When_GetBulkUploadRegistrationsTechSpecFileAsync_Is_Called
    {        
        [Fact]
        public void Then_DownloadFileAsync_Is_Called()
        {
            BlobStorageService.Received(1).DownloadFileAsync(Arg.Any<BlobStorageData>());
        }

        [Fact]
        public void Then_Expected_Stream_Is_Returned()
        {
            ActualResult.Should().NotBeNull();
        }
    }
}
