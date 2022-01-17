using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GenerateResultsExport
{
    public class When_Called_With_Data : TestSetup
    {
        private List<DataExportResponse> _expectedApiResult;

        public override void Given()
        {
            _expectedApiResult = new List<DataExportResponse>
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

            InternalApiClient.GenerateDataExportAsync(AoUkprn, DataExportType.Results, RequestedBy)
                .Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Should().BeEquivalentTo(_expectedApiResult);
        }
    }
}
