using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationHoursGet
{
    public class When_Cache_Doesnt_Contain_Hours : TestSetup
    {
        private readonly AdminChangeIpViewModel _cachedViewModel = new()
        {
            AdminIpCompletion = new AdminIpCompletionViewModel
            {
                RegistrationPathwayId = 1,
                IndustryPlacementStatusTo = IndustryPlacementStatus.CompletedWithSpecialConsideration
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

            model.RegistrationPathwayId.Should().Be(_cachedViewModel.AdminIpCompletion.RegistrationPathwayId);
            model.Hours.Should().BeNullOrEmpty();
        }
    }
}
