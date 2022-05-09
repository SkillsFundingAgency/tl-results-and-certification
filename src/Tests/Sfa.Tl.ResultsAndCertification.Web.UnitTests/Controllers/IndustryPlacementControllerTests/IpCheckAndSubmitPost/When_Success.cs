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
        private IpModelViewModel _ipModelViewModel;
        private IpTempFlexibilityUsedViewModel _ipTempFlexibilityUsedViewModel;

        private string _expectedSuccessBannerMsg;
        private string _expectedBannerHeaderMsg;

        public override void Given()
        {
            var isSuccess = true;

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

            _expectedSuccessBannerMsg = IpBannerContent.Success_Message;
            _expectedBannerHeaderMsg = IpBannerContent.Banner_HeaderMesage;
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {            
            IndustryPlacementLoader.Received(1).ProcessIndustryPlacementDetailsAsync(ProviderUkprn, _cacheModel);
            CacheService.Received(1).RemoveAsync<IndustryPlacementViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(TrainingProviderCacheKey, Arg.Is<NotificationBannerModel>(x => x.DisplayMessageBody == true && x.IsRawHtml == true && x.HeaderMessage.Equals(_expectedBannerHeaderMsg) && x.Message.Equals(_expectedSuccessBannerMsg)), CacheExpiryTime.XSmall);
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
