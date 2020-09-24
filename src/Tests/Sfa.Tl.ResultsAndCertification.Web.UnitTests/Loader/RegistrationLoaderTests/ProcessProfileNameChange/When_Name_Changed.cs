using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessProfileNameChange
{
    public class When_Specialism_Changed : TestSetup
    {
        RegistrationDetails mockRegDetails = null;
        readonly string reqFirstName = "John";
        readonly string reqLastName = "Smith";

        readonly string existingFirstName = "First";
        readonly string existingLastName = "Last";

        public override void Given()
        {
            var profileId = 1;
            var uln = 1234567890;

            ViewModel = new ChangeLearnersNameViewModel { ProfileId = profileId, Firstname = reqFirstName, Lastname = reqLastName };

            mockRegDetails = new RegistrationDetails
            {
                Firstname = existingFirstName,
                Lastname = existingLastName,
                Uln = uln,
                ProfileId = profileId,
            };

            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active)
                .Returns(mockRegDetails);

            InternalApiClient.UpdateRegistrationAsync(Arg.Is<ManageRegistration>
                (x => x.Uln == mockRegDetails.Uln &&
                x.ProfileId == mockRegDetails.ProfileId &&
                x.FirstName == ViewModel.Firstname && 
                x.LastName == ViewModel.Lastname))
                .Returns(true);
        }

        [Fact]
        public void Then_Called_ExpectedMethods()
        {
            InternalApiClient.Received().GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active);
            InternalApiClient.Received()
                .UpdateRegistrationAsync(Arg.Any<ManageRegistration>());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.IsModified.Should().BeTrue();
            ActualResult.IsSuccess.Should().BeTrue();

            ActualResult.Uln.Should().Be(mockRegDetails.Uln);
            ActualResult.ProfileId.Should().Be(mockRegDetails.ProfileId);
        }
    }
}
