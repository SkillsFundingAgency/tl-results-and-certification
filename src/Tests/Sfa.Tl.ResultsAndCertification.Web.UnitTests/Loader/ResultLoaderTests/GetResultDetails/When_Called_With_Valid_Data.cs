using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.ResultLoaderTests.GetResultDetails
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = new ResultDetails
            {
                ProfileId = 1,
                Uln = 1234567890,
                Firstname = "John",
                Lastname = "Smith",
                ProviderUkprn = 1234567,
                ProviderName = "Test Provider",               
                Status = RegistrationPathwayStatus.Active
            };

            InternalApiClient.GetResultDetailsAsync(AoUkprn, ProfileId).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.ProfileId.Should().Be(expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(expectedApiResult.Uln);
            ActualResult.Name.Should().Be(string.Concat(expectedApiResult.Firstname, " ", expectedApiResult.Lastname));
            ActualResult.ProviderDisplayName.Should().Be($"{expectedApiResult.ProviderName} ({expectedApiResult.ProviderUkprn})");           
            ActualResult.PathwayStatus.Should().Be(expectedApiResult.Status);
        }
    }
}
