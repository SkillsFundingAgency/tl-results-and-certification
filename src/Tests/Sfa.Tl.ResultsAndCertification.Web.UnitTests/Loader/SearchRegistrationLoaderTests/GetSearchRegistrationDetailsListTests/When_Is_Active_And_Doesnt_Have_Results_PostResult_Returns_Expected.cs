using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.SearchRegistration.Enum;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.SearchRegistrationLoaderTests.GetSearchRegistrationDetailsListTests
{
    public class When_Is_Active_And_Doesnt_Have_Results_PostResult_Returns_Expected : TestSetup
    {
        public override void Given()
            => Given(SearchRegistrationType.PostResult, isWithdrawn: false, hasResults: false);

        [Fact]
        public void Then_Expected_Methods_Called()
            => AssertExpectedMethodsCalled();

        [Fact]
        public void Then_Returns_Expected()
        {
            AssertResultExceptRouteName();
            Result.RegistrationDetails[0].Route.RouteName.Should().Be(RouteConstants.PrsNoResults);
        }
    }
}