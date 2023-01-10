using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkIndustryPlacementLoaderTests.Process
{
    public class When_IndustryPlacements_Stage3_Are_Validated : TestSetup
    {
        private List<IndustryPlacementCsvRecordResponse> expectedStage2Response;

        public override void Given()
        {
            expectedStage2Response = new List<IndustryPlacementCsvRecordResponse>
            {
                new IndustryPlacementCsvRecordResponse { RowNum = 1, Uln = 1111111111, CoreCode = "1234567", IndustryPlacementStatus = "Completed", ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "1", Uln = "1111111111", ErrorMessage = "Core code must have 8 digits only" }
                } },
                new IndustryPlacementCsvRecordResponse { RowNum = 2, Uln = 1111111112, CoreCode = "12345678", IndustryPlacementStatus = "Completed with special considerations", IndustryPlacementHours = "100", ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "2", Uln = "1111111112", ErrorMessage = "Special consideration reason needs to be provided" }
                } },
                new IndustryPlacementCsvRecordResponse { RowNum = 3, Uln = 1111111113, CoreCode = "12345678", IndustryPlacementStatus = "Completed" },
                new IndustryPlacementCsvRecordResponse { RowNum = 4, Uln = 1111111114, CoreCode = "12345678", IndustryPlacementStatus = "Comple" },
                new IndustryPlacementCsvRecordResponse { RowNum = 5, Uln = 1111111115, CoreCode = "12345678", IndustryPlacementStatus = "Completed with special considerations", IndustryPlacementHours = "500", SpecialConsiderations = new List<string> { "Xyz", "Abc" } }
            };

            var expectedStage3Response = new List<IndustryPlacementRecordResponse>
            {
                new IndustryPlacementRecordResponse { ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "3", Uln = "1111111113", ErrorMessage = "Core code either not recognised or not registered for this ULN" },
                } },
                new IndustryPlacementRecordResponse { ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "4", Uln = "1111111114", ErrorMessage = "Industry placement status is not valid"}
                } },
                new IndustryPlacementRecordResponse { ValidationErrors = new List<BulkProcessValidationError>
                {
                    new BulkProcessValidationError { RowNum = "5", Uln = "1111111115", ErrorMessage = "There is a problem with the special consideration code(s)"}
                } },
            };

            var csvResponse = new CsvResponseModel<IndustryPlacementCsvRecordResponse> { Rows = expectedStage2Response };
            var expectedWriteFileBytes = new byte[5];

            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            CsvService.ReadAndParseFileAsync(Arg.Any<IndustryPlacementCsvRecordRequest>()).Returns(csvResponse);
            IndustryPlacementService.ValidateIndustryPlacementsAsync(AoUkprn, Arg.Any<IEnumerable<IndustryPlacementCsvRecordResponse>>()).Returns(expectedStage3Response);
            CsvService.WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>()).Returns(expectedWriteFileBytes);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            BlobService.Received(1).DownloadFileAsync(Arg.Any<BlobStorageData>());
            CsvService.Received(1).ReadAndParseFileAsync(Arg.Any<IndustryPlacementCsvRecordRequest>());
            IndustryPlacementService.Received(1).ValidateIndustryPlacementsAsync(AoUkprn, Arg.Is<IEnumerable<IndustryPlacementCsvRecordResponse>>(x => x.All(r => r.IsValid)));
            IndustryPlacementService.DidNotReceive().TransformIndustryPlacementsModel(Arg.Any<IList<IndustryPlacementRecordResponse>>(), Arg.Any<string>());
            CsvService.Received(1).WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>());
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
            BlobService.Received(1).MoveFileAsync(Arg.Any<BlobStorageData>());
            DocumentUploadHistoryService.Received(1).CreateDocumentUploadHistory(Arg.Any<DocumentUploadHistoryDetails>());

            Response.IsSuccess.Should().BeFalse();
            Response.BlobUniqueReference.Should().Be(BlobUniqueRef);
        }
    }
}
