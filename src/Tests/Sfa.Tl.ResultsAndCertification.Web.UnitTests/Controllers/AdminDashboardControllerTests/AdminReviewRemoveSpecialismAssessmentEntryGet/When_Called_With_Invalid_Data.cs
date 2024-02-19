using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewRemoveSpecialismAssessmentEntryGet
{
    public class When_Called_With_Invalid_Data: TestSetup
    {
        protected AdminReviewRemoveSpecialismAssessmentEntryViewModel Mockresult = null;
        public override void Given()
        {
            CacheService.GetAsync<AdminReviewRemoveSpecialismAssessmentEntryViewModel>(CacheKey).Returns(Mockresult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
