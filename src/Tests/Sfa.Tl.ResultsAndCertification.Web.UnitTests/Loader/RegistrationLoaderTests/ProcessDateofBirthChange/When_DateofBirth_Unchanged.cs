using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessDateofBirthChange
{
    public class When_DateofBirth_Unchanged : TestSetup
    {
        ManageRegistration mockResponse = null;
        
        public override void Given()
        {
            ViewModel = new ChangeDateofBirthViewModel { Day = "1", Month = "2", Year = "2000" };
            mockResponse = new ManageRegistration 
            {
                DateOfBirth = ViewModel.DateofBirth.ToDateTime()
            };

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
