using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkRegistrationLoaderTests.ProcessBulkRegistrations
{
    public class When_File_IsDirty : TestSetup
    {
        public override void Given()
        {
            var errorMessage = "InvalidHeader";
            var csvResponse = new CsvResponseModel<RegistrationCsvRecordResponse> 
            {
                IsDirty = true,
                ErrorMessage = errorMessage,
            };

            var expectedWriteFileBytes = new byte[5];            
            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            CsvService.ReadAndParseFileAsync(Arg.Any<RegistrationCsvRecordRequest>()).Returns(csvResponse);
            CsvService.WriteFileAsync(Arg.Any<List<RegistrationValidationError>>()).Returns(expectedWriteFileBytes);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            CsvService.Received(1).ReadAndParseFileAsync(Arg.Any<RegistrationCsvRecordRequest>());
            CsvService.Received(1).WriteFileAsync(Arg.Any<List<RegistrationValidationError>>());
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
            BlobService.Received(1).MoveFileAsync(Arg.Any<BlobStorageData>());
            DocumentUploadHistoryService.Received(1).CreateDocumentUploadHistory(Arg.Any<DocumentUploadHistoryDetails>());
            
            Response.Result.IsSuccess.Should().BeFalse();
            Response.Result.BlobUniqueReference.Should().Be(BlobUniqueRef);
        }
    }
}
