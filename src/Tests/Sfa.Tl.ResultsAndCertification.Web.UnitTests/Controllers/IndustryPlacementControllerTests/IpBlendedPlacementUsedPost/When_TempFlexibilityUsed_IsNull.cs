using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpBlendedPlacementUsedPost
{
    public class When_TempFlexibilityUsed_IsNull : TestSetup
    {
        private IndustryPlacementViewModel _cacheResult;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpTempFlexibilityUsedViewModel _ipTempFlexibilityUsedViewModel;
        private IpTempFlexNavigation _ipTempFlexNavigation;

        public override void Given()
        {
            // Cache object
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, PathwayId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = IndustryPlacementStatus.Completed };
            _ipTempFlexibilityUsedViewModel = null;

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                IpModelViewModel = new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = false } },
                TempFlexibility = new IpTempFlexibilityViewModel { IpTempFlexibilityUsed = _ipTempFlexibilityUsedViewModel }
            };
            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);

            // Loader navigation
            _ipTempFlexNavigation = new IpTempFlexNavigation { AskBlendedPlacement = true, AskTempFlexibility = true };
            IndustryPlacementLoader.GetTempFlexNavigationAsync(_cacheResult.IpCompletion.PathwayId, _cacheResult.IpCompletion.AcademicYear).Returns(_ipTempFlexNavigation);

            ViewModel = new IpBlendedPlacementUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName, IsBlendedPlacementUsed = false };
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            IndustryPlacementLoader.Received(1).GetTempFlexNavigationAsync(_cacheResult.IpCompletion.PathwayId, _cacheResult.IpCompletion.AcademicYear);
        }

        [Fact]
        public void Then_Redirected_To_Expected_Route()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.IpCheckAndSubmit);
        }
    }
}
