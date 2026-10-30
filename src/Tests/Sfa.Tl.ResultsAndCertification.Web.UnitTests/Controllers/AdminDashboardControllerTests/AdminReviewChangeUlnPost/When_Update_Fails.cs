using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeUlnPost
{
    public class When_Update_Fails : TestSetup
    {
        public override void Given()
        {
            CacheService.GetAsync<AdminChangeUlnViewModel>(CacheKey).Returns(Entry());
            AdminDashboardLoader.ProcessChangeUlnAsync(Arg.Any<ReviewChangeUlnViewModel>()).Returns(false);
            AdminDashboardLoader.IsLearnerUlnAvailableAsync(9999999991).Returns(true);
            Model = Learner();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var result = Result.Should().BeOfType<RedirectToRouteResult>().Which;
            result.RouteName.Should().Be(RouteConstants.ProblemWithService);
        }
    }
}
