using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GetAssessmentsDataFile
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            BlobUniqueReference = Guid.NewGuid();
            ComponentType = Common.Enum.ComponentType.Core;
            BlobStorageService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test assessments entries data file")));
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            BlobStorageService.Received(1).DownloadFileAsync(Arg.Any<BlobStorageData>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
        }
    }
}
