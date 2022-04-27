using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using LearnerDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.LearnerRecordDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddEnglishStatusPost
{
    public class When_Success : TestSetup
    {
        private NotificationBannerModel _expectedSuccessBannerMsg;

        public override void Given()
        {
            ProfileId = 1;
            _expectedSuccessBannerMsg = new NotificationBannerModel { HeaderMessage = LearnerDetailsContent.Success_Header_English_Status_Added, Message = LearnerDetailsContent.Success_Message_English_Status_Added, DisplayMessageBody = true, IsRawHtml = true };

            ViewModel = new AddEnglishStatusViewModel { ProfileId = 1, LearnerName = "John Smith", IsAchieved = true };
            TrainingProviderLoader.UpdateLearnerSubjectAsync(ProviderUkprn, ViewModel).Returns(true);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).UpdateLearnerSubjectAsync(ProviderUkprn, ViewModel);

            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(x =>
                x.HeaderMessage.Equals(_expectedSuccessBannerMsg.HeaderMessage, System.StringComparison.InvariantCultureIgnoreCase) &&
                x.Message.Equals(_expectedSuccessBannerMsg.Message, System.StringComparison.InvariantCultureIgnoreCase) &&
                x.DisplayMessageBody == true &&
                x.IsRawHtml == true),
                CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_LearnerRecordDetails()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.LearnerRecordDetails);
        }
    }
}
