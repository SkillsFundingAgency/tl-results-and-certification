using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.ChangePostalAddress
{
    public class When_Action_IsCalled : TestSetup
    {
        public override void Given()
        {
            ProfileId = 11;
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as ChangePostalAddressViewModel;
            model.ProfileId.Should().Be(ProfileId);

            // Back link
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.RequestSoaCheckAndSubmit);
            model.BackLink.RouteAttributes.Count.Should().Be(1);
            model.BackLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string routeValue);
            routeValue.Should().Be(ProfileId.ToString());
        }
    }
}
