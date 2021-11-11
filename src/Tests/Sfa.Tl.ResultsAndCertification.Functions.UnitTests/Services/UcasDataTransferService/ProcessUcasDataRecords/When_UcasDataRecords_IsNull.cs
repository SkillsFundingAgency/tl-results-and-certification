using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Ucas;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.UcasDataTransferService.ProcessUcasDataRecords
{
    public class When_UcasDataRecords_IsNull : TestBase
    {
        private readonly UcasData _mockUcasData = null;

        public override void Given()
        {
            UcasDataType = UcasDataType.Entries;
            UcasDataService.ProcessUcasDataRecordsAsync(UcasDataType).Returns(_mockUcasData);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            UcasDataService.Received(1).ProcessUcasDataRecordsAsync(UcasDataType.Entries);
            UcasApiClient.DidNotReceive().SendDataAsync(Arg.Any<UcasDataRequest>());
            BlobStorageService.DidNotReceive().UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
        }

        [Fact]
        public void Then_Expected_Response_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsSuccess.Should().BeTrue();
            ActualResult.Message.Should().Be("No entries are found. Method: GetUcasEntriesAsync()");
        }
    }
}
