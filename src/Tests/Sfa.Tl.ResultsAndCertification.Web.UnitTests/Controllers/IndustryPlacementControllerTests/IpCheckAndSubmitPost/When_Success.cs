using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;
using IpBannerContent = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement.IndustryPlacementBanner;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCheckAndSubmitPost
{
    public class When_Success : TestSetup
    {
        private IndustryPlacementViewModel _cacheModel;
        private IpCompletionViewModel _ipCompletionViewModel;

        private NotificationBannerModel _expectedNotificationBannerModel;
        private string _expectedBannerHeaderMsg;

        public override void Given()
        {
            var isSuccess = true;

            // Cache object
            _ipCompletionViewModel = new IpCompletionViewModel { ProfileId = 1, AcademicYear = 2020, LearnerName = "First Last", IndustryPlacementStatus = IndustryPlacementStatus.Completed };

            _cacheModel = new IndustryPlacementViewModel
            {
                IpCompletion = _ipCompletionViewModel,
            };

            CacheService.GetAsync<IndustryPlacementViewModel>(CacheKey).Returns(_cacheModel);

            _expectedNotificationBannerModel = new NotificationBannerModel
            {
                HeaderMessage = IpBannerContent.Banner_HeaderMesage,
                Message = IpBannerContent.Success_Message_Completed,
                DisplayMessageBody = true,
                IsRawHtml = true
            };

            IndustryPlacementLoader.ProcessIndustryPlacementDetailsAsync(ProviderUkprn, _cacheModel).Returns(isSuccess);
            IndustryPlacementLoader.GetSuccessNotificationBanner(_ipCompletionViewModel.IndustryPlacementStatus).Returns(_expectedNotificationBannerModel);

            _expectedBannerHeaderMsg = IpBannerContent.Banner_HeaderMesage;
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {            
            IndustryPlacementLoader.Received(1).ProcessIndustryPlacementDetailsAsync(ProviderUkprn, _cacheModel);
            CacheService.Received(1).RemoveAsync<IndustryPlacementViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(TrainingProviderCacheKey, _expectedNotificationBannerModel, CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_Expected_Route()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.LearnerRecordDetails);
            route.RouteValues.Count.Should().Be(1);
            route.RouteValues[Constants.ProfileId].Should().Be(_cacheModel.IpCompletion.ProfileId);
        }        
    }
}
