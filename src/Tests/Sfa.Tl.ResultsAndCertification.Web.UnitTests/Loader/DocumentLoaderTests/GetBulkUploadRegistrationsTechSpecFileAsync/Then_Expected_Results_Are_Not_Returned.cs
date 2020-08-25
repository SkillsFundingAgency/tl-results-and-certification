using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using System.IO;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DocumentLoaderTests.GetBulkUploadRegistrationsTechSpecFileAsync
{
    public class Then_Expected_Results_Are_Not_Returned : When_GetBulkUploadRegistrationsTechSpecFileAsync_Is_Called
    {
        private Stream _fileStream;
        public override void Given()
        {
            _fileStream = null;
            FileName = null;           
            BlobStorageService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(_fileStream);
            Loader = new DocumentLoader(Logger, BlobStorageService);
        }

        [Fact]
        public void Then_DownloadFileAsync_Is_Not_Called()
        {
            BlobStorageService.DidNotReceive().DownloadFileAsync(Arg.Any<BlobStorageData>());
        }

        [Fact]
        public void Then_Expected_Stream_Is_Not_Returned()
        {
            ActualResult.Should().BeNull();
        }
    }
}
