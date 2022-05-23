using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpSpecialConsiderationHoursPost
{
    public class When_Called_With_Valid_Data_IsChangeMode_IsTrue : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private SpecialConsiderationHoursViewModel _specialConsiderationHoursViewModel;

        public override void Given()
        {
            _specialConsiderationHoursViewModel = new SpecialConsiderationHoursViewModel { LearnerName = "First Last", Hours = "999", IsChangeMode = true };
            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = new IpCompletionViewModel { IndustryPlacementStatus = Common.Enum.IndustryPlacementStatus.CompletedWithSpecialConsideration },
                SpecialConsideration = new SpecialConsiderationViewModel { Hours = _specialConsiderationHoursViewModel },
                IsChangeModeAllowed = true
            };

            ViewModel = _specialConsiderationHoursViewModel;
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.Received(1).SetAsync(CacheKey, Arg.Any<IndustryPlacementViewModel>());
        }

        [Fact]
        public void Then_Redirected_To_Expected_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.IpCheckAndSubmit);
        }
    }
}
