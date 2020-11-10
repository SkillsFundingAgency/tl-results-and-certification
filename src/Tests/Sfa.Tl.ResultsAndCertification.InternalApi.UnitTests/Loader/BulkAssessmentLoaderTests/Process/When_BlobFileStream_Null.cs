using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkAssessmentLoaderTests.Process
{
    public class When_BlobFileStream_Null : TestSetup
    {
        public override void Given()
        {
            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).ReturnsNull();
        }

        [Fact]
        public void Then_Throws_Exception()
        {
            Response.Should().Throws<Exception>();
        }
    }
}
