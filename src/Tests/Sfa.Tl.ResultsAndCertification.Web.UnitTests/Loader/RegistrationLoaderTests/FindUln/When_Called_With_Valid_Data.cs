using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.FindUln
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            expectedApiResult = new FindUlnResponse
            {
                RegistrationProfileId = 1,
                Uln = Uln,
                IsRegisteredWithOtherAo = true,
                Status = Common.Enum.RegistrationPathwayStatus.Active
            };
            
            InternalApiClient.FindUlnAsync(Ukprn, Uln).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var expectedIsAllowedValue = expectedApiResult.Status == Common.Enum.RegistrationPathwayStatus.Active || expectedApiResult.Status == Common.Enum.RegistrationPathwayStatus.Withdrawn;
            ActualResult.Should().NotBeNull();
            ActualResult.Uln.Should().Be(expectedApiResult.Uln.ToString());
            ActualResult.RegistrationProfileId.Should().Be(expectedApiResult.RegistrationProfileId);
            ActualResult.IsAllowed.Should().Be(expectedIsAllowedValue);
            ActualResult.IsRegisteredWithOtherAo.Should().Be(expectedApiResult.IsRegisteredWithOtherAo);
        }
    }
}
