using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Loader;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetResultValidationErrorsFile
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        public override void Given()
        {
            BlobUniqueReference = Guid.NewGuid();
            ApiResponse = new DocumentUploadHistoryDetails
            {
                AoUkprn = Ukprn,
                BlobFileName = BlobFileName,
                BlobUniqueReference = BlobUniqueReference,
                DocumentType = (int)DocumentType.Results,
                FileType = (int)FileType.Csv,
                Status = (int)DocumentUploadStatus.Processed,
                CreatedBy = $"{Givenname} {Surname}"
            };

            InternalApiClient.GetDocumentUploadHistoryDetailsAsync(Ukprn, BlobUniqueReference).Returns(ApiResponse);
            BlobStorageService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            Loader = new ResultLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_Exected_Methods_Called()
        {
            InternalApiClient.Received(1).GetDocumentUploadHistoryDetailsAsync(Ukprn, BlobUniqueReference);
        }

        [Fact]
        public void Then_Expected_Methods_NotCalled()
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
