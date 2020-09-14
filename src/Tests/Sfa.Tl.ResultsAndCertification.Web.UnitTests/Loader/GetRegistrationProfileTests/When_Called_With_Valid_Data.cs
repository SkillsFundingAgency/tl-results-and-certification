using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.GetRegistrationProfileTests
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private ManageRegistration mockResult;

        public override void Given()
        {
            mockResult = new ManageRegistration
            {
                ProfileId = 1, 
                FirstName = "John",
                LastName  = "Smith",
                PerformedBy = "updatedUser"
            };

            InternalApiClient.GetRegistrationAsync(AoUkprn, ProfileId).Returns(mockResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.ProfileId.Should().Be(mockResult.ProfileId);
            ActualResult.Firstname.Should().Be(mockResult.FirstName);
            ActualResult.Lastname.Should().Be(mockResult.LastName);
        }
    }
}
