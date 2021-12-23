using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AssessmentLoaderTests.GenerateAssessmentsExport
{
    public class When_Called_With_Data : TestSetup
    {
        public override void Given()
        {
            ExpectedApiResult = new List<DataExportResponse>
            {
                new DataExportResponse
                {
                    ComponentType = ComponentType.Core,
                    BlobUniqueReference = Guid.NewGuid(),
                    FileSize = 100,
                    IsDataFound = true
                },
                new DataExportResponse
                {
                    ComponentType = ComponentType.Specialism,
                    BlobUniqueReference = Guid.NewGuid(),
                    FileSize = 50,
                    IsDataFound = true
                }
            };

            InternalApiClient.GenerateDataExportAsync(AoUkprn, DataExportType.Assessments, RequestedBy)
                .Returns(ExpectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Should().BeEquivalentTo(ExpectedApiResult);
        }
    }
}
