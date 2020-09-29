using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessDateofBirthChange
{
    public class When_DateofBirth_Changed : TestSetup
    {
        RegistrationDetails mockRegDetails = null;

        public override void Given()
        {
            var profileId = 1;
            var uln = 1234567890;

            ViewModel = new ChangeDateofBirthViewModel { ProfileId = profileId, Day = "1", Month = "2", Year = "2000" };
            mockRegDetails = new RegistrationDetails
            {
                DateofBirth = DateTime.UtcNow.Date,
                Uln = uln,
                ProfileId = profileId,
            };

            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active)
                .Returns(mockRegDetails);

            InternalApiClient.UpdateRegistrationAsync(Arg.Is<ManageRegistration>
                (x => x.Uln == mockRegDetails.Uln &&
                x.ProfileId == mockRegDetails.ProfileId &&
                x.DateOfBirth == ViewModel.DateofBirth.ToDateTime()))
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
