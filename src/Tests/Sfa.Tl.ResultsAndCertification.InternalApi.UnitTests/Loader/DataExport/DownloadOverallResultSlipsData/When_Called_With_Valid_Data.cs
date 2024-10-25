using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.DataExport.DownloadOverallResultSlipsData
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private IList<Models.DownloadOverallResults.DownloadOverallResultSlipsData> _overallResultSlipsData;

        public override void Given()
        {
            List<Models.DownloadOverallResults.DownloadOverallResultSlipsData> _overallResultSlipsData = new()
            {
                new Models.DownloadOverallResults.DownloadOverallResultSlipsData
                {
                    Uln = 1234567890,
                    LearnerName = "John Smith",
                    OverallResult = "A",
                    CoreAssessmentSeries = "Summer 2022",
                    SpecialismAssessmentSeries= "Summer 2024",
                    ProviderName = "Provider Name",
                    ProviderUkprn = "1234567890",
                    Details = new Models.OverallResults.OverallResultDetail
                    {
                        PathwayLarId = "12345678",
                        PathwayName = "Construction",
                        PathwayResult = "B",
                        TlevelTitle = "T level in Construction",
                        IndustryPlacementStatus = "Completed",
                        SpecialismDetails = new List<Models.OverallResults.OverallSpecialismDetail>
                        {
                            new Models.OverallResults.OverallSpecialismDetail
                            {
                                SpecialismLarId = "23456789",
                                SpecialismName = "Plumbing",
                                SpecialismResult = "Merit"
                            }
                        }
                    }
                }
            };

            OverallResultCalculationService.DownloadOverallResultSlipsDataAsync(ProviderUkprn).Returns(_overallResultSlipsData);
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
            OverallResultCalculationService.Received(1).DownloadOverallResultSlipsDataAsync(ProviderUkprn);
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
        }
    }
}
