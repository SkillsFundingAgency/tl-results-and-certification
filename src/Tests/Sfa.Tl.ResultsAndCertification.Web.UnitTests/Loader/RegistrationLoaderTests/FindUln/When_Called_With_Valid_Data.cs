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
                IsRegisteredWithOtherAo = true
            };
            
            InternalApiClient.FindUlnAsync(Ukprn, Uln).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Uln.Should().Be(expectedApiResult.Uln.ToString());
            ActualResult.RegistrationProfileId.Should().Be(expectedApiResult.RegistrationProfileId);
            ActualResult.IsActive.Should().Be(expectedApiResult.IsActive);
            ActualResult.IsRegisteredWithOtherAo.Should().Be(expectedApiResult.IsRegisteredWithOtherAo);
        }
    }
}
