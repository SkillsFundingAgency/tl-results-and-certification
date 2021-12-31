using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System;
using System.IO;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetResultsDataFile
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            Stream stream = null;
            BlobUniqueReference = Guid.NewGuid();
            BlobStorageService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(stream);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            BlobStorageService.Received(1).DownloadFileAsync(Arg.Any<BlobStorageData>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
