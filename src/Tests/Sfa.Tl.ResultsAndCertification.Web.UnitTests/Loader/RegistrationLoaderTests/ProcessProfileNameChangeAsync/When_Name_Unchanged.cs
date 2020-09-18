using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessProfileNameChangeAsync
{
    public class When_Specialism_Unchanged : TestSetup
    {
        ManageRegistration mockResponse = null;
        readonly string firstName = " John";
        readonly string lastName = "Smith ";

        public override void Given()
        {
            ViewModel = new ChangeLearnersNameViewModel { ProfileId = 1, Firstname = firstName, Lastname = lastName };
            mockResponse = new ManageRegistration { FirstName = firstName.Trim().ToUpper(), LastName = lastName.Trim().ToUpper() };

            InternalApiClient.GetRegistrationAsync(AoUkprn, ViewModel.ProfileId)
                .Returns(mockResponse);
        }

        [Fact]
        public void Then_Called_GetRegistrationAsync()
        {
            InternalApiClient.Received().GetRegistrationAsync(AoUkprn, ViewModel.ProfileId);
            InternalApiClient.DidNotReceive().UpdateRegistrationAsync(Arg.Any<ManageRegistration>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsModified.Should().BeFalse();
        }
    }
}
