using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.GetRegistrationProfileTests
{
    public class Then_On_Invalid_ProfileId_Returns_Null : When_GetRegistrationProfileAsync_Is_Called
    {
        private readonly ManageRegistration mockResult = null;

        public override void Given()
        {
            InternalApiClient.GetRegistrationAsync(AoUkprn, ProfileId).Returns(mockResult);
        }

        [Fact]
        public void Then_Returns_Null_Result()
        {
            ActualResult.Should().BeNull();
        }
    }
}
