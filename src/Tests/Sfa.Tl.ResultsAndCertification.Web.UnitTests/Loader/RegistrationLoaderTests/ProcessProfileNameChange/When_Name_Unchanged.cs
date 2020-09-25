using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessProfileNameChange
{
    public class When_Specialism_Unchanged : TestSetup
    {
        RegistrationDetails regDetailsMock = null;
        readonly string firstName = " John";
        readonly string lastName = "Smith ";

        public override void Given()
        {
            ViewModel = new ChangeLearnersNameViewModel { ProfileId = 1, Firstname = firstName, Lastname = lastName };
            regDetailsMock = new RegistrationDetails { Firstname = firstName.Trim().ToUpper(), Lastname = lastName.Trim().ToUpper() };

            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active)
                .Returns(regDetailsMock);
        }

        [Fact]
        public void Then_Called_GetRegistrationAsync()
        {
            InternalApiClient.Received().GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active);
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
