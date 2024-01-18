using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationReasonsPost
{
    public class When_Cached_Hours_Empty : TestSetup
    {
        public override void Given()
        {
            const int RegistrationPathwayId = 1;

            var cachedAdminChangeIpViewModel = new AdminChangeIpViewModel
            {
                AdminIpCompletion = new AdminIpCompletionViewModel
                {
                    RegistrationPathwayId = RegistrationPathwayId,
                    IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration
                },
                HoursViewModel = new AdminIpSpecialConsiderationHoursViewModel
                {
                    RegistrationPathwayId = RegistrationPathwayId,
                    Hours = string.Empty
                }
            };

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

            CacheService.GetAsync<AdminChangeIpViewModel>(CacheKey).Returns(cachedAdminChangeIpViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).GetAsync<AdminChangeIpViewModel>(CacheKey);
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<AdminChangeIpViewModel>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            Result.ShouldBeRedirectPageNotFound();
        }
    }
}
