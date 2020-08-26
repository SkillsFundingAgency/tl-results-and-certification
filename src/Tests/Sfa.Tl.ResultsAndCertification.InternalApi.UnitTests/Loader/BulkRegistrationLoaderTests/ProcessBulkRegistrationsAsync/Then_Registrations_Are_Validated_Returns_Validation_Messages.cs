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

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkRegistrationLoaderTests.ProcessBulkRegistrationsAsync
{
    public class Then_Registrations_Are_Validated_Returns_Validation_Messages : When_ProcessBulkRegistrationsAsync_Is_Called
    {
        private List<RegistrationValidationError> expectedErrors;

        public override void Given()
        {

            var expectedWriteFileRequest = new List<RegistrationCsvRecordResponse>
                {
                    new RegistrationCsvRecordResponse { RowNum = 1, ProviderUkprn = 11 },
                    new RegistrationCsvRecordResponse { RowNum = 2, ProviderUkprn = 22, ValidationErrors = new List<RegistrationValidationError>
                    {
                        new RegistrationValidationError { RowNum = "1", Uln = "11", ErrorMessage = "First name required" },
                        new RegistrationValidationError { RowNum = "1", Uln = "11", ErrorMessage = "Core code required" }
                    } },
                    new RegistrationCsvRecordResponse { RowNum = 3, ProviderUkprn = 33, ValidationErrors = new List<RegistrationValidationError>
                    {
                        new RegistrationValidationError { RowNum = "3", Uln = "33", ErrorMessage = "Invalid Date"}
                    } },
                };

            var csvResponse = new CsvResponseModel<RegistrationCsvRecordResponse> { Rows = expectedWriteFileRequest };
            expectedErrors = ExtractExpectedErrors(csvResponse);

            var expectedWriteFileBytes = new byte[5];
            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            CsvService.ReadAndParseFileAsync(Arg.Any<RegistrationCsvRecordRequest>()).Returns(csvResponse);
            //CsvService.WriteFileAsync(Arg.Is(expectedErrors)).Returns(expectedWriteFileBytes);
            
            CsvService.WriteFileAsync(Arg.Any<List<RegistrationValidationError>>()).Returns(expectedWriteFileBytes);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
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
