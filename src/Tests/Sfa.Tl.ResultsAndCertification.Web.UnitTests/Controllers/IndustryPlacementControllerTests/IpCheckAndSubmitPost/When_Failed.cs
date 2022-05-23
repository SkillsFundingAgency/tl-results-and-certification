using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCheckAndSubmitPost
{
    public class When_Failed : TestSetup
    {
        private IndustryPlacementViewModel _cacheModel;
        private IpCompletionViewModel _ipCompletionViewModel;
        private IpModelViewModel _ipModelViewModel;
        private IpTempFlexibilityUsedViewModel _ipTempFlexibilityUsedViewModel;

        public override void Given()
        {
            var isSuccess = false;

            // Cache object
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = IndustryPlacementStatus.Completed };
            _ipModelViewModel = new IpModelViewModel { IpModelUsed = new IpModelUsedViewModel { IsIpModelUsed = false }, IpMultiEmployerUsed = new IpMultiEmployerUsedViewModel { IsMultiEmployerModelUsed = true } };
            _ipTempFlexibilityUsedViewModel = new IpTempFlexibilityUsedViewModel { LearnerName = _ipCompletionViewModel.LearnerName, IsTempFlexibilityUsed = false };

            _cacheModel = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
                IpModelViewModel = _ipModelViewModel,
                TempFlexibility = new IpTempFlexibilityViewModel { IpTempFlexibilityUsed = _ipTempFlexibilityUsedViewModel }
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheModel);

            IndustryPlacementLoader.ProcessIndustryPlacementDetailsAsync(ProviderUkprn, _cacheModel).Returns(isSuccess);
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            IndustryPlacementLoader.Received(1).ProcessIndustryPlacementDetailsAsync(ProviderUkprn, _cacheModel);
            CacheService.DidNotReceive().RemoveAsync<IndustryPlacementViewModel>(CacheKey);
            CacheService.DidNotReceive().SetAsync(TrainingProviderCacheKey, Arg.Any<NotificationBannerModel>());
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }       
    }
}
