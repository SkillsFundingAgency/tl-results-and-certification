using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessDateofBirthChangeAsync
{
    public class When_DateofBirth_NotFound : TestSetup
    {
        readonly ManageRegistration mockResponse = null;

        public override void Given()
        {
            ViewModel = new ChangeDateofBirthViewModel { ProfileId = 1 };
            InternalApiClient.GetRegistrationAsync(AoUkprn, Arg.Any<int>())
                .Returns(mockResponse);
        }

        [Fact]
        public void Then_Returns_Null()
        {
            InternalApiClient.Received().GetRegistrationAsync(AoUkprn, ViewModel.ProfileId);
            ActualResult.Should().BeNull();
        }
    }
}
