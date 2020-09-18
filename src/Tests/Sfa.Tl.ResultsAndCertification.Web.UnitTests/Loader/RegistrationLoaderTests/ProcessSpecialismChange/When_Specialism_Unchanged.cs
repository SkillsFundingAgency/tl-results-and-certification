using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessSpecialismChange
{
    public class When_Specialism_Unchanged : TestSetup
    {
        ManageRegistration mockResponse = null;

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

            mockResponse = new ManageRegistration
            {
                Uln = uln,
                ProfileId = profileId,
                SpecialismCodes = new List<string> { "111", "222" },
                PerformedBy = "Test user"
            };

            InternalApiClient.GetRegistrationAsync(AoUkprn, ViewModel.ProfileId)
                .Returns(mockResponse);

            InternalApiClient.UpdateRegistrationAsync(mockResponse)
                .Returns(true);
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
