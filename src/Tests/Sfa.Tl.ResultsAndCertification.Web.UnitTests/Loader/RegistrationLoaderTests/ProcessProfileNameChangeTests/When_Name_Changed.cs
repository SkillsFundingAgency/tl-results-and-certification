using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessProfileNameChangeTests
{
    public class When_Name_Changed : TestSetup
    {
        ManageRegistration mockResponse = null;
        readonly string reqFirstName = "John";
        readonly string reqLastName = "Smith";

        readonly string existingFirstName = "First";
        readonly string existingLastName = "Last";

        public override void Given()
        {
            var profileId = 1;
            var uln = 1234567890;

            ViewModel = new ChangeLearnersNameViewModel { ProfileId = profileId, Firstname = reqFirstName, Lastname = reqLastName };
            mockResponse = new ManageRegistration
            {
                FirstName = existingFirstName,
                LastName = existingLastName,
                Uln = uln,
                ProfileId = profileId,
                PerformedBy = "Test user"
            };

            InternalApiClient.GetRegistrationAsync(AoUkprn, ViewModel.ProfileId)
                .Returns(mockResponse);

            InternalApiClient.UpdateRegistrationAsync(mockResponse)
                .Returns(true);
        }

        [Fact]
        public void Then_Called_ExpectedMethods()
        {
            InternalApiClient.Received().GetRegistrationAsync(AoUkprn, ViewModel.ProfileId);
            InternalApiClient.Received().UpdateRegistrationAsync(mockResponse);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsModified.Should().BeTrue();
            ActualResult.IsSuccess.Should().BeTrue();

            ActualResult.Uln.Should().Be(mockResponse.Uln);
            ActualResult.ProfileId.Should().Be(mockResponse.ProfileId);
        }
    }
}
