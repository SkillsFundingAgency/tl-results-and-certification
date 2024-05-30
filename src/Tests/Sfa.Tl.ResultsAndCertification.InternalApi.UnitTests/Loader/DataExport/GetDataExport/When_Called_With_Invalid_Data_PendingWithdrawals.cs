using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.DataExport.GetDataExport
{
    public class When_Called_With_Invalid_Data_PendingWithdrawals : TestSetup
    {
        public override void Given()
        {
            DataExportType = DataExportType.PendingWithdrawals;
            DataExportRepository.GetDataExportRegistrationsAsync(AoUkprn).Returns(new List<RegistrationsExport>());
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            Response.Should().NotBeNull();
            Response.Should().HaveCount(1);
            Response[0].IsDataFound.Should().BeFalse();
            Response[0].ComponentType.Should().Be(ComponentType.NotSpecified);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            DataExportRepository.Received(1).GetDataExportPendingWithdrawalsAsync(AoUkprn);
            BlobService.DidNotReceive().UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
        }
    }
}