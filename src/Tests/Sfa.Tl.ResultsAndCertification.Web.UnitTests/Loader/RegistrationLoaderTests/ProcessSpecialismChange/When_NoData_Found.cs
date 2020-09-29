using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.RegistrationLoaderTests.ProcessSpecialismChange
{
    public class When_NoData_Found : TestSetup
    {
        readonly RegistrationDetails mockRegDetails = null;

        public override void Given()
        {
            ViewModel = new ChangeSpecialismViewModel { ProfileId = 1 };
            InternalApiClient.GetRegistrationDetailsAsync(AoUkprn, Arg.Any<int>(), RegistrationPathwayStatus.Active)
                .Returns(mockRegDetails);
        }

        [Fact]
        public void Then_Returns_Null()
        {
            InternalApiClient.Received().GetRegistrationDetailsAsync(AoUkprn, ViewModel.ProfileId, RegistrationPathwayStatus.Active);
            ActualResult.Should().BeNull();
        }
    }
}
