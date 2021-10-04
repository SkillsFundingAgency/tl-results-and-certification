using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegistrationAssessment
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = new AssessmentDetails
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                ProviderUkprn = 1234567,
                ProviderName = "Test Provider",
                PathwayLarId = "7654321",
                PathwayName = "Pathway",
                PathwayAssessmentSeries = "Summer 2021",
                PathwayAssessmentId = 1,
                SpecialismLarId = "2345678",
                SpecialismName = "Specialism1",
                SpecialismAssessmentSeries = "Autumn 2022",
                SpecialismAssessmentId = 25,
                Status = RegistrationPathwayStatus.Active,
                IsCoreEntryEligible = true
            };

            InternalApiClient.GetAssessmentDetailsAsync(AoUkprn, ProfileId).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.ProfileId.Should().Be(expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(expectedApiResult.Uln);
            ActualResult.Name.Should().Be(string.Concat(expectedApiResult.Firstname, " ", expectedApiResult.Lastname));
            ActualResult.ProviderDisplayName.Should().Be($"{expectedApiResult.ProviderName} ({expectedApiResult.ProviderUkprn})");
            ActualResult.PathwayDisplayName.Should().Be($"{expectedApiResult.PathwayName} ({expectedApiResult.PathwayLarId})");
            ActualResult.PathwayAssessmentSeries.Should().Be(expectedApiResult.PathwayAssessmentSeries);
            ActualResult.IsResultExist.Should().BeFalse();

            var expectedSpecialismDisplayName = !string.IsNullOrWhiteSpace(expectedApiResult.SpecialismLarId) ? $"{expectedApiResult.SpecialismName} ({expectedApiResult.SpecialismLarId})" : null;
            ActualResult.SpecialismDisplayName.Should().Be(expectedSpecialismDisplayName);
            ActualResult.SpecialismAssessmentSeries.Should().Be(expectedApiResult.SpecialismAssessmentSeries);
            ActualResult.PathwayStatus.Should().Be(expectedApiResult.Status);
        }
    }
}
