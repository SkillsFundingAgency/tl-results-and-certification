using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationHoursGet
{
    public class When_Cache_Contains_Hours : TestSetup
    {
        private readonly AdminChangeIpViewModel _cachedViewModel = new()
        {
            AdminIpCompletion = new AdminIpCompletionViewModel
            {
                RegistrationPathwayId = 1,
                IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration
            },
            HoursViewModel = new AdminIpSpecialConsiderationHoursViewModel
            {
                RegistrationPathwayId = 1,
                Hours = "50"
            }
        };

        public override void Given()
        {
            CacheService.GetAsync<AdminChangeIpViewModel>(CacheKey).Returns(_cachedViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangeIpViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminIpSpecialConsiderationHoursViewModel>();

            AdminIpSpecialConsiderationHoursViewModel cachedHoursModel = _cachedViewModel.HoursViewModel;
            model.Should().Be(cachedHoursModel);
            model.RegistrationPathwayId.Should().Be(cachedHoursModel.RegistrationPathwayId);
            model.Hours.Should().Be(cachedHoursModel.Hours);
        }
    }
}
