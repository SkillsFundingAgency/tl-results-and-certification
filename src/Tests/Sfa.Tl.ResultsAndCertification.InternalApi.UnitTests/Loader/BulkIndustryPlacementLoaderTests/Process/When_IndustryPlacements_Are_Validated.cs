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
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using IndustryPlacementStatus = Sfa.Tl.ResultsAndCertification.Common.Enum.IndustryPlacementStatus;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkIndustryPlacementLoaderTests.Process
{
    public class When_IndustryPlacements_Are_Validated : TestSetup
    {
        private List<IndustryPlacementRecordResponse> expectedStage3Response;

        public override void Given()
        {
            var expectedStage2Response = new List<IndustryPlacementCsvRecordResponse>
            {
                new IndustryPlacementCsvRecordResponse { RowNum = 1, Uln = 1111111111, CoreCode = "12345678", IndustryPlacementStatus = "Completed" },
                new IndustryPlacementCsvRecordResponse { RowNum = 2, Uln = 1111111112, CoreCode = "12345679", IndustryPlacementStatus = "Completed with special considerations", IndustryPlacementHours = "500", SpecialConsiderations = new List<string>{ "1","2" } },
                new IndustryPlacementCsvRecordResponse { RowNum = 3, Uln = 1111111113, CoreCode = "12345680", IndustryPlacementStatus = "Not completed" }
            };

            expectedStage3Response = new List<IndustryPlacementRecordResponse>
            {
                new IndustryPlacementRecordResponse { TqRegistrationPathwayId = 1, IpStatus = (int)IndustryPlacementStatus.Completed },
                new IndustryPlacementRecordResponse { TqRegistrationPathwayId = 3, IpStatus = (int)IndustryPlacementStatus.CompletedWithSpecialConsideration, IpHours = 500, SpecialConsiderationReasons = new List<int?> { 1, 2 } },
                new IndustryPlacementRecordResponse { TqRegistrationPathwayId = 3, IpStatus = (int)IndustryPlacementStatus.NotCompleted }
            };

            var csvResponse = new CsvResponseModel<IndustryPlacementCsvRecordResponse> { Rows = expectedStage2Response };
            var expectedWriteFileBytes = new byte[5];

            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            CsvService.ReadAndParseFileAsync(Arg.Any<IndustryPlacementCsvRecordRequest>()).Returns(csvResponse);
            IndustryPlacementService.ValidateIndustryPlacementsAsync(AoUkprn, Arg.Any<IEnumerable<IndustryPlacementCsvRecordResponse>>()).Returns(expectedStage3Response);
            CsvService.WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>()).Returns(expectedWriteFileBytes);

            var transformationResponse = GetTransformIndustryPlacementsModel();
            IndustryPlacementService.TransformIndustryPlacementsModel(expectedStage3Response, Arg.Any<string>()).Returns(transformationResponse);

            var IndustryPlacementsProcessResult = new IndustryPlacementProcessResponse { IsSuccess = true };
            IndustryPlacementService.CompareAndProcessIndustryPlacementsAsync(transformationResponse).Returns(IndustryPlacementsProcessResult);

        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            BlobService.Received(1).DownloadFileAsync(Arg.Any<BlobStorageData>());
            CsvService.Received(1).ReadAndParseFileAsync(Arg.Any<IndustryPlacementCsvRecordRequest>());
            IndustryPlacementService.Received(1).ValidateIndustryPlacementsAsync(AoUkprn, Arg.Any<IEnumerable<IndustryPlacementCsvRecordResponse>>());

            BlobService.Received(1).MoveFileAsync(Arg.Any<BlobStorageData>());
            DocumentUploadHistoryService.Received(1).CreateDocumentUploadHistory(Arg.Any<DocumentUploadHistoryDetails>());

            Response.IsSuccess.Should().BeTrue();
            Response.ErrorFileSize.Should().Be(0);
            Response.Stats.Should().NotBeNull();
            Response.Stats.TotalRecordsCount.Should().Be(expectedStage3Response.Count);
        }

        private IList<IndustryPlacement> GetTransformIndustryPlacementsModel()
        {
            var industryPlacements = new List<IndustryPlacement> 
            { 
                new IndustryPlacement { TqRegistrationPathwayId = 1, Status = IndustryPlacementStatus.Completed },
                new IndustryPlacement { TqRegistrationPathwayId = 2, Status = IndustryPlacementStatus.CompletedWithSpecialConsideration, Details = "IP" },
                new IndustryPlacement { TqRegistrationPathwayId = 3, Status = IndustryPlacementStatus.NotCompleted },
            };

            return industryPlacements;
        }
    }
}