using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System.IO;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetPendingWithdrawalsDataFile
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            BlobStorageService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            BlobStorageService.Received(1).DownloadFileAsync(Arg.Is<BlobStorageData>(b =>
                b.ContainerName == DocumentType.DataExports.ToString()
                && b.BlobFileName == $"{BlobUniqueReference}.{FileType.Csv}"
                && b.SourceFilePath == $"{AoUkprn}/{DataExportType.PendingWithdrawals}"));
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
        }
    }
}