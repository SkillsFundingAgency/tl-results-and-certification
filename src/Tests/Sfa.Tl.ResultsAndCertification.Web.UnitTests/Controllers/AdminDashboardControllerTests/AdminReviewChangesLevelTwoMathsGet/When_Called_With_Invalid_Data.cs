using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangesLevelTwoMathsGet
{
    public class When_Called_With_Invalid_Data : TestSetup
    {
        protected AdminReviewChangesMathsSubjectViewModel _mockResult = null;

        public override void Given()
        {
            PathwayId = 0;
            AdminDashboardLoader.GetAdminLearnerRecordAsync<AdminReviewChangesMathsSubjectViewModel>(PathwayId).Returns(_mockResult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}