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
using System;
using Sfa.Tl.ResultsAndCertification.Domain.Models;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkAssessmentLoaderTests.Process
{
    public class When_Assessments_Are_Validated : TestSetup
    {
        private List<AssessmentRecordResponse> expectedStage3Response;

        public override void Given()
        {
            var expectedStage2Response = new List<AssessmentCsvRecordResponse>
            {
                new AssessmentCsvRecordResponse { RowNum = 1, Uln = 1111111111, CoreCode = "12345678", CoreAssessmentEntry = "Summer 2022" },
                new AssessmentCsvRecordResponse { RowNum = 2, Uln = 1111111112, SpecialismCodes = "LAR12345", SpecialismAssessmentEntry = "Autumn 2023" },
                new AssessmentCsvRecordResponse { RowNum = 3, Uln = 1111111113, CoreCode = "12345678", CoreAssessmentEntry = "Summer 2022", SpecialismCodes = "LAR12345", SpecialismAssessmentEntry = "Autumn 2023" }
            };

            expectedStage3Response = new List<AssessmentRecordResponse>
            {
                new AssessmentRecordResponse { TqRegistrationPathwayId = 1, PathwayAssessmentSeriesId = 11 },
                new AssessmentRecordResponse { TqRegistrationSpecialismIds = 2, SpecialismAssessmentSeriesId = 22 },
                new AssessmentRecordResponse { TqRegistrationPathwayId = 3, PathwayAssessmentSeriesId = 33, TqRegistrationSpecialismIds = 333, SpecialismAssessmentSeriesId = 3333 }
            };

            var csvResponse = new CsvResponseModel<AssessmentCsvRecordResponse> { Rows = expectedStage2Response };
            var expectedWriteFileBytes = new byte[5];

            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            CsvService.ReadAndParseFileAsync(Arg.Any<AssessmentCsvRecordRequest>()).Returns(csvResponse);
            AssessmentService.ValidateAssessmentsAsync(AoUkprn, Arg.Any<IEnumerable<AssessmentCsvRecordResponse>>()).Returns(expectedStage3Response);
            CsvService.WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>()).Returns(expectedWriteFileBytes);

            var transformationResponse = GetTransformAssessmentsModel();
            AssessmentService.TransformAssessmentsModel(expectedStage3Response, Arg.Any<string>()).Returns(transformationResponse);

            var assessmentsProcessResult = new AssessmentProcessResponse { IsSuccess = true };
            AssessmentService.CompareAndProcessAssessmentsAsync(transformationResponse.Item1, transformationResponse.Item2).
                Returns(assessmentsProcessResult);

        }
        
        [Fact]
        public void Then_Returns_Expected_Results()
        {
            BlobService.Received(1).DownloadFileAsync(Arg.Any<BlobStorageData>());
            CsvService.Received(1).ReadAndParseFileAsync(Arg.Any<AssessmentCsvRecordRequest>());
            AssessmentService.Received(1).ValidateAssessmentsAsync(AoUkprn, Arg.Any<IEnumerable<AssessmentCsvRecordResponse>>());
            
            BlobService.Received(1).MoveFileAsync(Arg.Any<BlobStorageData>());
            DocumentUploadHistoryService.Received(1).CreateDocumentUploadHistory(Arg.Any<DocumentUploadHistoryDetails>());

            Response.IsSuccess.Should().BeTrue();
            Response.ErrorFileSize.Should().Be(0);
            Response.Stats.Should().NotBeNull();
            Response.Stats.TotalRecordsCount.Should().Be(expectedStage3Response.Count);
        }

        private (IList<TqPathwayAssessment>, IList<TqSpecialismAssessment>) GetTransformAssessmentsModel()
        {
            var pathwayAssessments = new List<TqPathwayAssessment> { new TqPathwayAssessment { TqRegistrationPathwayId = 1, AssessmentSeriesId = 2 } };
            var specialismAssessments = new List<TqSpecialismAssessment> { new TqSpecialismAssessment { TqRegistrationSpecialismId = 11, AssessmentSeriesId = 22 } };

            return (pathwayAssessments, specialismAssessments);
        }
    }
}
