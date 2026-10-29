using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeUlnGet
{
    public class When_Cache_Is_Empty : TestSetup
    {
        public override void Given()
        {
            AdminDashboardLoader.GetAdminLearnerRecordAsync<ReviewChangeUlnViewModel>(3).Returns(Learner());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var result = Result.Should().BeOfType<RedirectToRouteResult>().Which;
            result.RouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
