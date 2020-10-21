using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessSpecialismChange
{
    public class When_Specialism_Unchanged : TestSetup
    {
        RegistrationDetails mockRegDetails = null;

        public override void Given()
        {
            var profileId = 1;
            var uln = 1234567890;

            ViewModel = new ChangeSpecialismViewModel
            {
                ProfileId = profileId,
                PathwaySpecialisms = new PathwaySpecialismsViewModel
                {
                    PathwayId = profileId,
                    Specialisms = new List<SpecialismDetailsViewModel> // UnChanged 
                    {
                        new SpecialismDetailsViewModel { Code = "111", IsSelected = true },
                        new SpecialismDetailsViewModel { Code = "222", IsSelected = true }
                    }
                }
            };

            mockRegDetails = new RegistrationDetails
            {
                Uln = uln,
                ProfileId = profileId,
                Specialisms = ViewModel.PathwaySpecialisms.Specialisms
                .Select(x => new SpecialismDetails { Code = x.Code }),
            };

            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active)
                .Returns(mockRegDetails);

            InternalApiClient.UpdateRegistrationAsync(Arg.Any<ManageRegistration>())
                .Returns(true);
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
