using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkAssessmentLoaderTests.Process
{
    public class When_Assessments_Are_Validated : TestSetup
    {
        public override void Given()
        {
            var expectedWriteFileRequest = new List<AssessmentCsvRecordResponse>
                {
                    new AssessmentCsvRecordResponse { RowNum = 1, Uln = 11 },
                    new AssessmentCsvRecordResponse { RowNum = 2, Uln = 22, ValidationErrors = new List<BulkProcessValidationError>
                    {
                        new BulkProcessValidationError { RowNum = "1", Uln = "11", ErrorMessage = "ULN must be a 10 digit number" },
                        new BulkProcessValidationError { RowNum = "1", Uln = "11", ErrorMessage = "Core code must have 8 digits only" }
                    } },
                    new AssessmentCsvRecordResponse { RowNum = 3, Uln = 33, ValidationErrors = new List<BulkProcessValidationError>
                    {
                        new BulkProcessValidationError { RowNum = "3", Uln = "33", ErrorMessage = "ULN must be a 10 digit number"}
                    } },
                };

            var csvResponse = new CsvResponseModel<AssessmentCsvRecordResponse> { Rows = expectedWriteFileRequest };
            var expectedWriteFileBytes = new byte[5];
            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            CsvService.ReadAndParseFileAsync(Arg.Any<AssessmentCsvRecordRequest>()).Returns(csvResponse);
            CsvService.WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>()).Returns(expectedWriteFileBytes);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            CsvService.Received(1).ReadAndParseFileAsync(Arg.Any<AssessmentCsvRecordRequest>());
            CsvService.Received(1).WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>());
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
            BlobService.Received(1).MoveFileAsync(Arg.Any<BlobStorageData>());
            DocumentUploadHistoryService.Received(1).CreateDocumentUploadHistory(Arg.Any<DocumentUploadHistoryDetails>());

            Response.IsSuccess.Should().BeFalse();
            Response.BlobUniqueReference.Should().Be(BlobUniqueRef);
        }
    }
}
