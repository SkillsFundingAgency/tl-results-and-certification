using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationReasonsPost
{
    public class When_CompletedWithSpecialConsideration : TestSetup
    {
        private const int RegistrationPathwayId = 120;

        private readonly AdminChangeIpViewModel _cachedViewModel = new AdminChangeIpViewModel
        {
            AdminIpCompletion = new AdminIpCompletionViewModel
            {
                RegistrationPathwayId = RegistrationPathwayId,
                IndustryPlacementStatusTo = IndustryPlacementStatus.CompletedWithSpecialConsideration
            },
            HoursViewModel = new AdminIpSpecialConsiderationHoursViewModel
            {
                RegistrationPathwayId = RegistrationPathwayId,
                Hours = "275"
            }
        };

        public override void Given()
        {
            ViewModel = new AdminIpSpecialConsiderationReasonsViewModel
            {
                RegistrationPathwayId = RegistrationPathwayId,
                ReasonsList = new List<IpLookupDataViewModel>
                    {
                        new IpLookupDataViewModel
                        {
                            Id = 1,
                            Name = "Domestic crisis",
                            IsSelected = true
                        }
                    }
            };

            CacheService.GetAsync<AdminChangeIpViewModel>(CacheKey).Returns(_cachedViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangeIpViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<AdminChangeIpViewModel>(p => p == _cachedViewModel && p.ReasonsViewModel == ViewModel));
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            // TODO: Update the route when the review industry placement page is built.
            Result.ShouldBeRedirectToRouteResult(nameof(RouteConstants.AdminReviewChangesIndustryPlacement));
        }
    }
}