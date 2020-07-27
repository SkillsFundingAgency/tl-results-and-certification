using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.GetRegistrationDetailsByProfileIdAsync
{
    public class Then_On_Invalid_ProfileId_No_Results_Are_Returned : When_GetRegistrationDetailsByProfileIdAsync_Is_Called
    {
        public override void Given()
        {
            expectedApiResult = null;
            InternalApiClient.GetRegistrationDetailsByProfileIdAsync(AoUkprn, ProfileId).Returns(expectedApiResult);
        }

        [Fact]
        public void Then_No_Results_Are_Returned()
        {
            ActualResult.Should().BeNull();
        }
    }
}
