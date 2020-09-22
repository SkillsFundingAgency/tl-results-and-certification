using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegistrationDetailsByProfileId
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = new RegistrationDetails
            {
                ProfileId = 1,
                Uln = 1234567890,
                Name = "Test",
                DateofBirth = DateTime.UtcNow,
                ProviderName = "Test Provider (1234567)",
                PathwayName = "Pathway (7654321)",
                SpecialismsDisplayName = new List<string> { "Specialism1 (2345678)", "Specialism2 (555678)" },
                AcademicYear = 2020,
                Status = RegistrationPathwayStatus.Active
            };

            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, ProfileId).Returns(expectedApiResult);
        }

        [Fact(Skip = "Todo Ravi")]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Uln.Should().Be(expectedApiResult.Uln);

            ActualResult.Uln.Should().Be(expectedApiResult.Uln);
            ActualResult.Name.Should().Be(expectedApiResult.Name);
            ActualResult.DateofBirth.Should().Be(expectedApiResult.DateofBirth);
            ActualResult.ProviderDisplayName.Should().Be(expectedApiResult.ProviderName);
            ActualResult.PathwayDisplayName.Should().Be(expectedApiResult.PathwayName);
            ActualResult.SpecialismsDisplayName.Should().BeEquivalentTo(expectedApiResult.SpecialismsDisplayName);
            ActualResult.AcademicYear.Should().Be(expectedApiResult.AcademicYear);
            ActualResult.Status.Should().Be(expectedApiResult.Status);
        }
    }
}
