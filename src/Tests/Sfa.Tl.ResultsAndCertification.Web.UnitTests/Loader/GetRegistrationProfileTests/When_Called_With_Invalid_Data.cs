using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.GetRegistrationProfileTests
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        private readonly ManageRegistration mockResult = null;

        public override void Given()
        {
            InternalApiClient.GetRegistrationAsync(AoUkprn, ProfileId).Returns(mockResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().BeNull();
        }
    }
}
