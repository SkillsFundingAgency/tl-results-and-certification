using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BlobStorage;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.InternalApi.UnitTests.Loader.DataExport.DownloadOverallResultsData
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private IList<Models.DownloadOverallResults.DownloadOverallResultsData> _overallResultsData;

        public override void Given()
        {
            _overallResultsData = new List<Models.DownloadOverallResults.DownloadOverallResultsData>
            {
                new Models.DownloadOverallResults.DownloadOverallResultsData
                {
                    Uln = 1234567890,
                    FirstName = "John",
                    LastName = "Smith",
                    AcademicYear = 2022,
                    DateOfBirth = DateTime.UtcNow.AddYears(-18),
                    OverallResult = "A",
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

            OverallResultCalculationService.DownloadOverallResultsDataAsync(ProviderUkprn, Arg.Any<DateTime>()).Returns(_overallResultsData);
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
            OverallResultCalculationService.Received(1).DownloadOverallResultsDataAsync(ProviderUkprn, Arg.Any<DateTime>());
            BlobService.Received(1).UploadFromByteArrayAsync(Arg.Any<BlobStorageData>());
        }
    }
}
