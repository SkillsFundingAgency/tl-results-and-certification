using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.Cache;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard;
using Xunit;
namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.ReviewChangeStartYearGet
{
    public class When_Called_With_Invalid_Data: TestSetup
    {
        protected AdminChangeStartYearViewModel AdminChangeStartYearViewModel = null;
        protected ReviewChangeStartYearViewModel Mockresult = null;
        
        public override void Given()
        {
            PathwayId = 0;
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.Received(1).GetAdminLearnerRecordAsync<ReviewChangeStartYearViewModel>(PathwayId);
            CacheService.Received(1).GetAsync<AdminChangeStartYearViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}
