using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.DataExport.DownloadOverallResultSlipsData
{
    public class When_Called_With_Empty_Data : TestSetup
    {
        private IList<Models.DownloadOverallResults.DownloadOverallResultSlipsData> _overallResultSlipsData;

        public override void Given()
        {
            _overallResultSlipsData = new List<Models.DownloadOverallResults.DownloadOverallResultSlipsData>();
            DownloadOverallResultsService.DownloadOverallResultSlipsDataAsync(ProviderUkprn, Arg.Any<DateTime>()).Returns(_overallResultSlipsData);
            ResultSlipsGeneratorService.GetByteData(_overallResultSlipsData).Returns(new byte[10]);
        }

        [Fact]
        public void Then_Expected_Results_Returned()
        {
            Response.Should().NotBeNull();
            Response.IsDataFound.Should().BeTrue();
            Response.ComponentType.Should().Be(Common.Enum.ComponentType.NotSpecified);
            Response.BlobUniqueReference.Should().NotBeEmpty();
            Response.FileSize.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Called()
        {
            DownloadOverallResultsService.Received(1).DownloadOverallResultSlipsDataAsync(ProviderUkprn, Arg.Any<DateTime>());
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
        }
    }
}
