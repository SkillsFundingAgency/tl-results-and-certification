using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using System.Linq;
using Sfa.Tl.ResultsAndCertification.Common.Constants;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.BulkResultLoaderTests.Process
{
    public class When_File_HasNoRecords : TestSetup
    {
        private CsvResponseModel<ResultCsvRecordResponse> csvResponse;

        public override void Given()
        {
            csvResponse = new CsvResponseModel<ResultCsvRecordResponse>
            {
                IsDirty = true,
                ErrorMessage = ValidationMessages.InvalidColumnFound,
                ErrorCode = CsvFileErrorCode.NoRecordsFound
            };

            var expectedWriteFileBytes = new byte[5];
            BlobService.DownloadFileAsync(Arg.Any<BlobStorageData>()).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File")));
            CsvService.ReadAndParseFileAsync(Arg.Any<ResultCsvRecordRequest>()).Returns(csvResponse);
            CsvService.WriteFileAsync(Arg.Any<List<BulkProcessValidationError>>()).Returns(expectedWriteFileBytes);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            CsvService.Received(1).ReadAndParseFileAsync(Arg.Any<ResultCsvRecordRequest>());
            CsvService.Received(1).WriteFileAsync(Arg.Is<List<BulkProcessValidationError>>(x => x.First().ErrorMessage.Equals(ValidationMessages.AtleastOneEntryRequired)));
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
            BlobService.Received(1).MoveFileAsync(Arg.Any<BlobStorageData>());
            DocumentUploadHistoryService.Received(1).CreateDocumentUploadHistory(Arg.Any<DocumentUploadHistoryDetails>());

            Response.IsSuccess.Should().BeFalse();
            Response.BlobUniqueReference.Should().Be(BlobUniqueRef);
        }
    }
}
