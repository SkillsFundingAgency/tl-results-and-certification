using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminReviewChangeIndustryPlacementPost
{
    public class When_Success : TestSetup
    {
        private AdminChangeIpViewModel _cacheModel;

        public override void Given()
        {
            ViewModel = CreateViewModel(IndustryPlacementStatus.Completed);

            _cacheModel = new AdminChangeIpViewModel()
            {
                AdminIpCompletion = new AdminIpCompletionViewModel()
                {
                    RegistrationPathwayId = 1,
                    IndustryPlacementStatusTo = IndustryPlacementStatus.Completed
                }
            };

            CacheService.GetAsync<AdminChangeIpViewModel>(CacheKey).Returns(_cacheModel);
            AdminDashboardLoader.ProcessChangeIndustryPlacementAsync(ViewModel).Returns(true);
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            AdminDashboardLoader.Received(1).ProcessChangeIndustryPlacementAsync(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_Expected_ActionResult()
        {
            var route = ActualResult as RedirectToActionResult;
            route.ActionName.Should().Be(RouteConstants.AdminLearnerRecord);
            route.RouteValues[Constants.PathwayId].Should().Be(_cacheModel.AdminIpCompletion.RegistrationPathwayId);
        }
    }
}