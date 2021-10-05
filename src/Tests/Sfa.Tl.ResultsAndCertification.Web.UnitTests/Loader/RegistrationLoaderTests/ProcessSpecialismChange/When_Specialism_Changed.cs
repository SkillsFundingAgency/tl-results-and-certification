using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
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
        List<string> _expectedSpecialismCodes;
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
                        new SpecialismDetailsViewModel { Code = "111|555", IsSelected = true }
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

            _expectedSpecialismCodes = new List<string>();
            ViewModel.PathwaySpecialisms.Specialisms.Where(x => x.IsSelected).Select(s => s.Code).ToList().ForEach(c => { _expectedSpecialismCodes.AddRange(c.Split(Constants.PipeSeperator)); });

            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active)
                .Returns(mockRegDetails);

            InternalApiClient.UpdateRegistrationAsync(Arg.Is<ManageRegistration>
                (x => x.Uln == mockRegDetails.Uln &&
                x.ProfileId == mockRegDetails.ProfileId &&
                x.SpecialismCodes.All(s => _expectedSpecialismCodes.Contains(s))))
                .Returns(true);
        }

        [Fact]
        public void Then_Called_ExpectedMethods()
        {
            InternalApiClient.Received().GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active);
            InternalApiClient.Received().UpdateRegistrationAsync(Arg.Any<ManageRegistration>());
        }
        [Fact]
        public void Then_Mapper_Returns_Expected_Results()
        {           

            var result = Mapper.Map<ManageRegistration>(mockRegDetails);
            Mapper.Map(ViewModel, result);

            result.Should().NotBeNull();
            result.HasSpecialismsChanged.Should().BeTrue();
            result.SpecialismCodes.Should().BeEquivalentTo(_expectedSpecialismCodes);
            result.PerformedBy.Should().Be($"{Givenname} {Surname}");
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
