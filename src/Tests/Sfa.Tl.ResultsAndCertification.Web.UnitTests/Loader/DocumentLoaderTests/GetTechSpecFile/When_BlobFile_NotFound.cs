using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System.IO;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.DocumentLoaderTests.GetTechSpecFile
{
    public class When_BlobFile_NotFound : TestSetup
    {
        public override void Given()
        {
            Stream stream = null;
            BlobStorageService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(stream);
        }

        [Fact]
        public void Then_Expected_Method_IsCalled()
        {
            BlobStorageService.Received(1).DownloadFileAsync(Arg.Any<BlobStorageData>());
        }

        [Fact]
        public void Then_Returns_Expected_Result()
        {
            ActualResult.Should().BeNull();
        }
    }
}
