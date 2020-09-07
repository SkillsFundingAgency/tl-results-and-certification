using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.GetRegistrationProfileTests
{
    public class Then_On_Valid_ProfiledId_Expected_Results_Are_Returned : When_GetRegistrationProfileAsync_Is_Called
    {
        private ManageRegistration mockResult;

        public override void Given()
        {
            mockResult = new ManageRegistration
            {
                ProfileId = 1, 
                FirstName = "John",
                LastName  = "Smith",
                ModifiedBy = "updatedUser"
            };

            InternalApiClient.GetRegistrationAsync(AoUkprn, ProfileId).Returns(mockResult);
        }

        [Fact]
        public void Then_Expected_Result_Is_Returned()
        {
            ActualResult.Should().NotBeNull();

            ActualResult.ProfileId.Should().Be(mockResult.ProfileId);
            ActualResult.Firstname.Should().Be(mockResult.FirstName);
            ActualResult.Lastname.Should().Be(mockResult.LastName);
        }
    }
}
