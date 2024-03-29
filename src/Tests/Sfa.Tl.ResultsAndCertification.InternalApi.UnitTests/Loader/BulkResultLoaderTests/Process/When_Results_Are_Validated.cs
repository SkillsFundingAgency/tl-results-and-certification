﻿using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkResultLoaderTests.Process
{
    public class When_Results_Are_Validated : TestSetup
    {
        private List<ResultRecordResponse> _expectedStage3Response;
        private (IList<TqPathwayResult>, IList<TqSpecialismResult>) _transformationResponse;

        public override void Given()
        {
            var expectedStage2Response = new List<ResultCsvRecordResponse>
            {
                new ResultCsvRecordResponse { RowNum = 1, Uln = 1111111111,  },
                new ResultCsvRecordResponse { RowNum = 2, Uln = 1111111112,  },
                new ResultCsvRecordResponse { RowNum = 3, Uln = 1111111113,  }
            };

            _expectedStage3Response = new List<ResultRecordResponse>
            {
                new ResultRecordResponse { TqPathwayAssessmentId = 1, PathwayComponentGradeLookupId = 1 },
                new ResultRecordResponse { TqPathwayAssessmentId = 2, PathwayComponentGradeLookupId = 2 },
                new ResultRecordResponse { TqPathwayAssessmentId = 3, PathwayComponentGradeLookupId = 3 }
            };

            var csvResponse = new CsvResponseModel<ResultCsvRecordResponse> { Rows = expectedStage2Response };
            var expectedWriteFileBytes = new byte[5];

            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            CsvService.ReadAndParseFileAsync(Arg.Any<ResultCsvRecordRequest>()).Returns(csvResponse);
            ResultService.ValidateResultsAsync(AoUkprn, Arg.Any<IEnumerable<ResultCsvRecordResponse>>()).Returns(_expectedStage3Response);
            CsvService.WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>()).Returns(expectedWriteFileBytes);

            _transformationResponse = GetTransformResultsModel();
            ResultService.TransformResultsModel(_expectedStage3Response, Arg.Any<string>()).Returns(_transformationResponse);

            var resultsProcessResult = new ResultProcessResponse { IsSuccess = true };
            ResultService.CompareAndProcessResultsAsync(_transformationResponse.Item1, _transformationResponse.Item2).Returns(resultsProcessResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            BlobService.Received(1).DownloadFileAsync(Arg.Any<BlobStorageData>());
            CsvService.Received(1).ReadAndParseFileAsync(Arg.Any<ResultCsvRecordRequest>());
            ResultService.Received(1).ValidateResultsAsync(AoUkprn, Arg.Any<IEnumerable<ResultCsvRecordResponse>>());
            ResultService.Received(1).TransformResultsModel(_expectedStage3Response, Arg.Any<string>());
            ResultService.Received(1).CompareAndProcessResultsAsync(_transformationResponse.Item1, _transformationResponse.Item2);
            BlobService.Received(1).MoveFileAsync(Arg.Any<BlobStorageData>());
            DocumentUploadHistoryService.Received(1).CreateDocumentUploadHistory(Arg.Any<DocumentUploadHistoryDetails>());

            Response.IsSuccess.Should().BeTrue();
            Response.Stats.Should().NotBeNull(); 
            Response.ErrorFileSize.Should().Be(0);
        }

        private (IList<TqPathwayResult>, IList<TqSpecialismResult>) GetTransformResultsModel()
        {
            var pathwayResults = new List<TqPathwayResult> { new TqPathwayResult { TqPathwayAssessmentId = 1, TlLookupId = 1 } };
            var specialismResults = new List<TqSpecialismResult> { new TqSpecialismResult { TqSpecialismAssessmentId = 1, TlLookupId = 1 } };

            return (pathwayResults, specialismResults);
        }
    }
}
