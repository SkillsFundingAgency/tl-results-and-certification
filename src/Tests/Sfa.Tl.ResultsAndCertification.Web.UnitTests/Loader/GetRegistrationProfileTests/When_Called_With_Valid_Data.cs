using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.GetRegistrationProfileTests
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private RegistrationDetails mockResult;

        public override void Given()
        {
            mockResult = new RegistrationDetails
            {
                ProfileId = 1, 
                Firstname = "John",
                Lastname  = "Smith",
            };

            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, ProfileId, RegistrationPathwayStatus.Active).Returns(mockResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.ProfileId.Should().Be(mockResult.ProfileId);
            ActualResult.Firstname.Should().Be(mockResult.Firstname);
            ActualResult.Lastname.Should().Be(mockResult.Lastname);
        }
    }
}
