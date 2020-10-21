using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessDateofBirthChange
{
    public class When_DateofBirth_Unchanged : TestSetup
    {
        RegistrationDetails regDetailsMock = null;

        public override void Given()
        {
            ViewModel = new ChangeDateofBirthViewModel { Day = "1", Month = "2", Year = "2000" };
            
            regDetailsMock = new RegistrationDetails
            {
                DateofBirth = ViewModel.DateofBirth.ToDateTime(),
                ProfileId = ViewModel.ProfileId,
            };

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
