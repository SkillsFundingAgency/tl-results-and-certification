using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.DataExport.DownloadOverallResultsData
{
    public class When_Called_With_Empty_Data : TestSetup
    {
        private IList<Models.DownloadOverallResults.DownloadOverallResultsData> _overallResultsData;

        public override void Given()
        {
            _overallResultsData = new List<Models.DownloadOverallResults.DownloadOverallResultsData>();
            OverallResultCalculationService.DownloadOverallResultsDataAsync(ProviderUkprn).Returns(_overallResultsData);
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
            OverallResultCalculationService.Received(1).DownloadOverallResultsDataAsync(ProviderUkprn);
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
        }
    }
}
