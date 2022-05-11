using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpCompletionPost
{
    public class When_IpStatus_CompletedWithSC_And_IsChangeMode_IsTrue : TestSetup
    {
        private IpCompletionViewModel _ipCompletionViewModel;
        private IndustryPlacementViewModel _cacheResult;
        private string _expectedSuccessBannerMsg;
        private string _expectedBannerHeaderMsg;

        public override void Given()
        {
            var isSuccess = true;

            _ipCompletionViewModel = new IpCompletionViewModel
            {
                ProfileId = 1,
                RegistrationPathwayId = 1,
                PathwayId = 7,
                AcademicYear = 2020,
                LearnerName = "First Last",
                IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration,
                IsChangeMode = true
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
            _expectedSuccessBannerMsg = IndustryPlacementBanner.Success_Message;
            _expectedBannerHeaderMsg = IndustryPlacementBanner.Banner_HeaderMesage;
        }

        [Fact]
        public void Then_Redirected_To_Expected_Route()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.LearnerRecordDetails);
            route.RouteValues.Count.Should().Be(1);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }

        [Fact]
        public void Then_Expected_Method_Are_Called()
        {
            IndustryPlacementLoader.Received(1).ProcessIndustryPlacementDetailsAsync(ProviderUkprn, _cacheResult);
            CacheService.Received(1).RemoveAsync<IndustryPlacementViewModel>(CacheKey);
            CacheService.Received(1).SetAsync(TrainingProviderCacheKey, Arg.Is<NotificationBannerModel>(x => x.HeaderMessage.Equals(_expectedBannerHeaderMsg) && x.Message.Equals(_expectedSuccessBannerMsg) && x.DisplayMessageBody == true && x.IsRawHtml == true), CacheExpiryTime.XSmall);
        }
    }
}
