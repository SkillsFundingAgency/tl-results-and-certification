using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.FindUlnAsync
{
    public class Then_Expected_Results_Are_Returned : When_FindUlnAsync_Is_Called
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
        public void Then_Expected_Result_Is_Returned()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.Uln.Should().Be(expectedApiResult.Uln.ToString());
            ActualResult.RegistrationProfileId.Should().Be(expectedApiResult.RegistrationProfileId);
            ActualResult.IsActive.Should().Be(expectedApiResult.IsActive);
            ActualResult.IsRegisteredWithOtherAo.Should().Be(expectedApiResult.IsRegisteredWithOtherAo);
        }
    }
}
