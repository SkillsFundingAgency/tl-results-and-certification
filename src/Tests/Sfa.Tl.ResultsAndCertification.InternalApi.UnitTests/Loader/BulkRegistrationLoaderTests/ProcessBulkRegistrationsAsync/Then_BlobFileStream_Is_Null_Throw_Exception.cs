using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkRegistrationLoaderTests.ProcessBulkRegistrationsAsync
{
    public class Then_BlobFileStream_Is_Null_Throw_Exception : When_ProcessBulkRegistrationsAsync_Is_Called
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
