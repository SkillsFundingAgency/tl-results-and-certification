using AutoMapper.Internal;
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
    public class When_Specialism_Changed : TestSetup
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
                    Specialisms = new List<SpecialismDetailsViewModel> // Changed 
                    { 
                        new SpecialismDetailsViewModel { Code = "111", IsSelected = true },
                        new SpecialismDetailsViewModel { Code = "555", IsSelected = true }
                    } 
                }
            };

            mockRegDetails = new RegistrationDetails
            {
                Uln = uln,
                ProfileId = profileId,
                Specialisms = ViewModel.PathwaySpecialisms.Specialisms
                .Select(x => new SpecialismDetails { Code = "different" }),
            };

            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active)
                .Returns(mockRegDetails);

            InternalApiClient.UpdateRegistrationAsync(Arg.Is<ManageRegistration>
                (x => x.Uln == mockRegDetails.Uln &&
                x.ProfileId == mockRegDetails.ProfileId &&
                x.SpecialismCodes.All(s => ViewModel.PathwaySpecialisms.Specialisms
                    .Select(c => c.Code).Contains(s))))
                .Returns(true);
        }

        [Fact]
        public void Then_Called_ExpectedMethods()
        {
            InternalApiClient.Received().GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active);
            InternalApiClient.Received().UpdateRegistrationAsync(Arg.Any<ManageRegistration>());
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
