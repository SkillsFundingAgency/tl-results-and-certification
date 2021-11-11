using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Ucas;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Functions.UnitTests.Services.UcasDataTransferService.ProcessUcasDataRecords
{
    public class When_UcasDataRecords_AreEmpty : TestBase
    {
        private UcasData _mockUcasData;

        public override void Given()
        {
            _mockUcasData = new UcasData
            {
                Header = new UcasDataHeader(),
                Trailer = new UcasDataTrailer(),
                UcasDataRecords = new List<UcasDataRecord>()
            };

            UcasDataType = UcasDataType.Entries;
            UcasDataService.ProcessUcasDataRecordsAsync(UcasDataType.Entries).Returns(_mockUcasData);
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
