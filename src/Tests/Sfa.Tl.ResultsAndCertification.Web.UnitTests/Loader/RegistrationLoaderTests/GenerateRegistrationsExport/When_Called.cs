using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.DataExport;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GenerateRegistrationsExport
{
    public class When_Called : TestSetup
    {
        public override void Given()
        {
            ExpectedApiResult = new List<DataExportResponse>
            {
                new DataExportResponse
                {
                    BlobUniqueReference = Guid.NewGuid(),
                    FileSize = 100,
                    IsDataFound = true
                }
            };

            InternalApiClient.GenerateDataExportAsync(AoUkprn, DataExportType.Registrations, RequestedBy)
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
