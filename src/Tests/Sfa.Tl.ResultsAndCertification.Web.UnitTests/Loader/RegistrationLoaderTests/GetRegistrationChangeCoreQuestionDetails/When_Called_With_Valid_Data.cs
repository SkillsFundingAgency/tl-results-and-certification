using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using System;
using System.Collections.Generic;
using Xunit;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegistrationChangeCoreQuestionDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = new RegistrationDetails
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.UtcNow,
                ProviderName = "Test Provider (1234567)",
                PathwayName = "Pathway (7654321)",
                Specialisms = new List<SpecialismDetails> { new SpecialismDetails { Code = "2345678", Name = "Specialism1" }, new SpecialismDetails { Code = "55567", Name = "Specialism2" } },
                AcademicYear = 2020,
                Status = RegistrationPathwayStatus.Active
            };

            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, ProfileId).Returns(expectedApiResult);
        }

        [Fact(Skip = "TODO Ravi")]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(expectedApiResult.ProfileId);
            ActualResult.ProviderDisplayName.Should().Be(expectedApiResult.ProviderName);
        }
    }
}
