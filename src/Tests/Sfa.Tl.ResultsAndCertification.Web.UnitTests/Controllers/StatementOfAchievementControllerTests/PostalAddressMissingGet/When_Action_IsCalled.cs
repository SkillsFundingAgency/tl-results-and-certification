using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.PostalAddressMissingGet
{
    public class When_Action_IsCalled : TestSetup
    {
        public override void Given(){}

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as PostalAddressMissingViewModel;
            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.Home);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
