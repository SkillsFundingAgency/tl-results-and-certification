using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkAssessmentLoaderTests.Process
{
    public class When_Assessments_Stage3_Are_Validated : TestSetup
    {
        private List<AssessmentCsvRecordResponse> expectedStage2Response;

        public override void Given()
        {
            expectedStage2Response = new List<AssessmentCsvRecordResponse>
            {
                new AssessmentCsvRecordResponse { RowNum = 1, Uln = 1111111111, CoreCode = "12345678", CoreAssessmentEntry = "Summer 2022", SpecialismCodes = "LAR12345", SpecialismAssessmentEntry = "Autumn 2023", ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "1", Uln = "1111111111", ErrorMessage = "Core code must have 8 digits only" }
                } },
                new AssessmentCsvRecordResponse { RowNum = 2, Uln = 1111111112, CoreCode = "12345678", CoreAssessmentEntry = "Summer 2022", SpecialismCodes = "LAR12345", SpecialismAssessmentEntry = "Autumn 2023", ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "2", Uln = "1111111112", ErrorMessage = "Last name required" }
                } },
                new AssessmentCsvRecordResponse { RowNum = 3, Uln = 1111111113, CoreCode = "12345678", CoreAssessmentEntry = "Summer 2022", SpecialismCodes = "LAR12345", SpecialismAssessmentEntry = "Autumn 2023" },
                new AssessmentCsvRecordResponse { RowNum = 4, Uln = 1111111114, CoreCode = "12345678", CoreAssessmentEntry = "Summer 2022", SpecialismCodes = "LAR12345", SpecialismAssessmentEntry = "Autumn 2023" },
                new AssessmentCsvRecordResponse { RowNum = 5, Uln = 1111111115, CoreCode = "12345678", CoreAssessmentEntry = "Summer 2022", SpecialismCodes = "LAR12345", SpecialismAssessmentEntry = "Autumn 2023" }
            };

            var expectedStage3Response = new List<AssessmentRecordResponse>
            {
                new AssessmentRecordResponse { ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "3", Uln = "1111111113", ErrorMessage = "Core code either not recognised or not registered for this ULN" },
                } },
                new AssessmentRecordResponse { ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "4", Uln = "1111111114", ErrorMessage = "Specialism code either not recognised or not registered for this ULN"}
                } },
                new AssessmentRecordResponse { ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "5", Uln = "1111111115", ErrorMessage = "Specialism assessment entry must be the second series in the second academic year or the first series in the third academic year and be in the format detailed in the 'format rules' and 'example' columns in the technical specification"}
                } },
            };

            var csvResponse = new CsvResponseModel<AssessmentCsvRecordResponse> { Rows = expectedStage2Response };
            var expectedWriteFileBytes = new byte[5];

            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            CsvService.ReadAndParseFileAsync(Arg.Any<AssessmentCsvRecordRequest>()).Returns(csvResponse);
            AssessmentService.ValidateAssessmentsAsync(AoUkprn, Arg.Any<IEnumerable<AssessmentCsvRecordResponse>>()).Returns(expectedStage3Response);
            CsvService.WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>()).Returns(expectedWriteFileBytes);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            BlobService.Received(1).DownloadFileAsync(Arg.Any<BlobStorageData>());
            CsvService.Received(1).ReadAndParseFileAsync(Arg.Any<AssessmentCsvRecordRequest>());
            AssessmentService.Received(1).ValidateAssessmentsAsync(AoUkprn, Arg.Is<IEnumerable<AssessmentCsvRecordResponse>>(x => x.All(r => r.IsValid)));
            AssessmentService.DidNotReceive().TransformAssessmentsModel(Arg.Any<IList<AssessmentRecordResponse>>(), Arg.Any<string>());
            CsvService.Received(1).WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>());
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
            BlobService.Received(1).MoveFileAsync(Arg.Any<BlobStorageData>());
            DocumentUploadHistoryService.Received(1).CreateDocumentUploadHistory(Arg.Any<DocumentUploadHistoryDetails>());

            Response.IsSuccess.Should().BeFalse();
            Response.BlobUniqueReference.Should().Be(BlobUniqueRef);
        }
    }
}