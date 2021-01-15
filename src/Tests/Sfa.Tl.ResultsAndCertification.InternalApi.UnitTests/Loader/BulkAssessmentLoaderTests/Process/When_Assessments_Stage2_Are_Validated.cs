using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkAssessmentLoaderTests.Process
{
    public class When_Assessments_Stage2_Are_Validated : TestSetup
    {
        private List<AssessmentCsvRecordResponse> expectedStage2Response;

        public override void Given()
        {
            expectedStage2Response = new List<AssessmentCsvRecordResponse>
            {
                new AssessmentCsvRecordResponse { RowNum = 1, ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "1", ErrorMessage = "TestErrorMessage1" }
                } },
                new AssessmentCsvRecordResponse { RowNum = 2, ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "2", ErrorMessage = "TestErrorMessage2" }
                } }
            };

            var csvResponse = new CsvResponseModel<AssessmentCsvRecordResponse> { Rows = expectedStage2Response };
            CsvService.ReadAndParseFileAsync(Arg.Any<AssessmentCsvRecordRequest>()).Returns(csvResponse);
            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            
            var expectedWriteFileBytes = new byte[5];
            CsvService.WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>()).Returns(expectedWriteFileBytes);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            CsvService.Received(1).ReadAndParseFileAsync(Arg.Any<AssessmentCsvRecordRequest>());
            AssessmentService.DidNotReceive().ValidateAssessmentsAsync(AoUkprn, Arg.Is<IEnumerable<AssessmentCsvRecordResponse>>(x => x.All(r => r.IsValid)));
            CsvService.Received(1).WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>());
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
            BlobService.Received(1).MoveFileAsync(Arg.Any<BlobStorageData>());
            DocumentUploadHistoryService.Received(1).CreateDocumentUploadHistory(Arg.Any<DocumentUploadHistoryDetails>());

            Response.IsSuccess.Should().BeFalse();
            Response.BlobUniqueReference.Should().Be(BlobUniqueRef);
        }
    }
}
