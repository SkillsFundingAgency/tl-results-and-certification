using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkResultLoaderTests.Process
{
    public class When_Results_Stage3_Are_Validated : TestSetup
    {
        private List<ResultCsvRecordResponse> expectedStage2Response;

        public override void Given()
        {
            expectedStage2Response = new List<ResultCsvRecordResponse>
            {
                new ResultCsvRecordResponse { RowNum = 1, Uln = 1111111111, CoreCode = "1234567", CoreAssessmentSeries = "Summer 2021", CoreGrade = "A", ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "1", Uln = "1111111111", ErrorMessage = "Core component code must have 8 digits only" }
                } },
                new ResultCsvRecordResponse { RowNum = 2, Uln = 1111111112, CoreCode = "", CoreAssessmentSeries = "Summer 2021", CoreGrade = "A", ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "2", Uln = "1111111112", ErrorMessage = "Core component code required when result is included" }
                } },
                new ResultCsvRecordResponse { RowNum = 3, Uln = 1111111113, CoreCode = "00000000", CoreAssessmentSeries = "Summer 2022", CoreGrade = "A" },
                new ResultCsvRecordResponse { RowNum = 4, Uln = 1111111114, CoreCode = "12345678", CoreAssessmentSeries = "xyz 2022", CoreGrade = "A" },
                new ResultCsvRecordResponse { RowNum = 5, Uln = 1111111115, CoreCode = "12345678", CoreAssessmentSeries = "Summer 2022", CoreGrade = "Z" }
            };

            var expectedStage3Response = new List<ResultRecordResponse>
            {
                new ResultRecordResponse { ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "3", Uln = "1111111113", ErrorMessage = ValidationMessages.InvalidCoreComponentCode },
                } },
                new ResultRecordResponse { ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "4", Uln = "1111111114", ErrorMessage = ValidationMessages.InvalidCoreAssessmentSeriesEntry}
                } },
                new ResultRecordResponse { ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "5", Uln = "1111111115", ErrorMessage = ValidationMessages.InvalidCoreComponentGrade}
                } },
            };

            var csvResponse = new CsvResponseModel<ResultCsvRecordResponse> { Rows = expectedStage2Response };
            var expectedWriteFileBytes = new byte[5];

            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            CsvService.ReadAndParseFileAsync(Arg.Any<ResultCsvRecordRequest>()).Returns(csvResponse);
            ResultService.ValidateResultsAsync(AoUkprn, Arg.Any<IEnumerable<ResultCsvRecordResponse>>()).Returns(expectedStage3Response);
            CsvService.WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>()).Returns(expectedWriteFileBytes);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            BlobService.Received(1).DownloadFileAsync(Arg.Any<BlobStorageData>());
            CsvService.Received(1).ReadAndParseFileAsync(Arg.Any<ResultCsvRecordRequest>());
            ResultService.Received(1).ValidateResultsAsync(AoUkprn, Arg.Is<IEnumerable<ResultCsvRecordResponse>>(x => x.All(r => r.IsValid)));
            CsvService.Received(1).WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>());
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
            BlobService.Received(1).MoveFileAsync(Arg.Any<BlobStorageData>());
            DocumentUploadHistoryService.Received(1).CreateDocumentUploadHistory(Arg.Any<DocumentUploadHistoryDetails>());

            Response.IsSuccess.Should().BeFalse();
            Response.BlobUniqueReference.Should().Be(BlobUniqueRef);
        }
    }
}
