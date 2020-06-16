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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegistrationValidationErrorsFileAsync
{
    public class Then_Expected_Results_Are_Not_Returned : When_GetRegistrationValidationErrorsFileAsync_Is_Called
    {
        public override void Given()
        {
            BlobUniqueReference = Guid.NewGuid();
            ApiResponse = new DocumentUploadHistoryDetails
            {
                AoUkprn = Ukprn,
                BlobFileName = BlobFileName,
                BlobUniqueReference = BlobUniqueReference,
                DocumentType = (int)DocumentType.Registrations,
                FileType = (int)FileType.Csv,
                Status = (int)DocumentUploadStatus.Processed,
                CreatedBy = $"{Givenname} {Surname}"
            };

            InternalApiClient.GetDocumentUploadHistoryDetailsAsync(Ukprn, BlobUniqueReference).Returns(ApiResponse);
            BlobStorageService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            Loader = new RegistrationLoader(Mapper, Logger, InternalApiClient, BlobStorageService);
        }

        [Fact]
        public void Then_GetDocumentUploadHistoryDetailsAsync_Is_Called()
        {
            InternalApiClient.Received(1).GetDocumentUploadHistoryDetailsAsync(Ukprn, BlobUniqueReference);
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
