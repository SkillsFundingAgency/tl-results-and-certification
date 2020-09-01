using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegistrationDetailsByProfileIdAsync
{
    public class Then_On_Valid_ProfileId_Expected_Results_Are_Returned : When_GetRegistrationDetailsByProfileIdAsync_Is_Called
    {
        public override void Given()
        {
            expectedApiResult = new RegistrationDetails
            {
                ProfileId = 1,
                Uln = 1234567890,
                Name = "Test",
                DateofBirth = DateTime.UtcNow,
                ProviderDisplayName = "Test Provider (1234567)",
                PathwayDisplayName = "Pathway (7654321)",
                SpecialismsDisplayName = new List<string> { "Specialism1 (2345678)", "Specialism2 (555678)" },
                AcademicYear = 2020,
                Status = RegistrationPathwayStatus.Active
            };

            InternalApiClient.GetRegistrationDetailsByProfileIdAsync(AoUkprn, ProfileId).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Expected_Result_Is_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Uln.Should().Be(expectedApiResult.Uln);

            ActualResult.Uln.Should().Be(expectedApiResult.Uln);
            ActualResult.Name.Should().Be(expectedApiResult.Name);
            ActualResult.DateofBirth.Should().Be(expectedApiResult.DateofBirth);
            ActualResult.ProviderDisplayName.Should().Be(expectedApiResult.ProviderDisplayName);
            ActualResult.PathwayDisplayName.Should().Be(expectedApiResult.PathwayDisplayName);
            ActualResult.SpecialismsDisplayName.Should().BeEquivalentTo(expectedApiResult.SpecialismsDisplayName);
            ActualResult.AcademicYear.Should().Be(expectedApiResult.AcademicYear);
            ActualResult.Status.Should().Be(expectedApiResult.Status);
        }
    }
}
