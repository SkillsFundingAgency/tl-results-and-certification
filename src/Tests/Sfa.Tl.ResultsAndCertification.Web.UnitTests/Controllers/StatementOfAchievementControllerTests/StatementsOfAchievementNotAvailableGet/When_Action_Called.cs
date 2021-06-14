using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.StatementOfAchievementControllerTests.StatementsOfAchievementNotAvailableGet
{
    public class When_Action_Called : TestSetup
    {
        public override void Given() { }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            (Result as ViewResult).Model.Should().NotBeNull();

            var model = (Result as ViewResult).Model as NotAvailableViewModel;
            model.Should().NotBeNull();
            model.BackLink.Should().NotBeNull();
            model.SoaAvailableDate.Should().Be(ResultsAndCertificationConfiguration.SoaAvailableDate);
            model.BackLink.RouteName.Should().Be(RouteConstants.Home);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
