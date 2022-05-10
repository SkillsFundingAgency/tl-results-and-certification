using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCompletionPost
{
    public class When_IpStatus_NotCompleted_Failed : TestSetup
    {
        private IpCompletionViewModel _ipCompletionViewModel;
        private IndustryPlacementViewModel _cacheResult;

        public override void Given()
        {
            var isSuccess = false;

            _ipCompletionViewModel = new IpCompletionViewModel
            {
                ProfileId = 1,
                RegistrationPathwayId = 1,
                PathwayId = 7,
                AcademicYear = 2020,
                LearnerName = "First Last",
                IndustryPlacementStatus = null
            };

            ViewModel = new IpCompletionViewModel
            {
                ProfileId = ProfileId,
                RegistrationPathwayId = 1,
                PathwayId = 7,
                AcademicYear = 2020,
                LearnerName = "First Last",
                IndustryPlacementStatus = IndustryPlacementStatus.NotCompleted
            };

            _cacheResult = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheResult);

            IndustryPlacementLoader.ProcessIndustryPlacementDetailsAsync(ProviderUkprn, _cacheResult).Returns(isSuccess);
        }

        [Fact]
        public void Then_Redirected_To_ProblemWithService()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.ProblemWithService);
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            IndustryPlacementLoader.Received(1).ProcessIndustryPlacementDetailsAsync(ProviderUkprn, _cacheResult);
            CacheService.DidNotReceive().RemoveAsync<IndustryPlacementViewModel>(CacheKey);
            CacheService.DidNotReceive().SetAsync(TrainingProviderCacheKey, Arg.Any<NotificationBannerModel>(), CacheExpiryTime.XSmall);
        }
    }
}
