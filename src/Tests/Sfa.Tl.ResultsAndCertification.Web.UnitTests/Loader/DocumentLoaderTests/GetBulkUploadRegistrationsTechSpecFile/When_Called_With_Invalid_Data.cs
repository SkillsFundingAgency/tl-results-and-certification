using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using System.IO;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DocumentLoaderTests.GetBulkUploadRegistrationsTechSpecFile
{
    public class When_Called_With_Invalid_Data : TestSetup
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
        public void Then_Expected_Methods_Not_Called()
        {
            BlobStorageService.DidNotReceive().DownloadFileAsync(Arg.Any<BlobStorageData>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
