using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using LearnerDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.TrainingProvider.LearnerRecordDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddMathsStatusPost
{
    public class When_Success : TestSetup
    {
        private string _expectedSuccessBannerMsg;

        public override void Given()
        {
            ProfileId = 1;

            _addMathsStatusViewModel = new AddMathsStatusViewModel
            {
                ProfileId = ProfileId,
                LearnerName = "John Smith",
                IsAchieved = true
            };

            _expectedSuccessBannerMsg = string.Format(LearnerDetailsContent.Success_Message_Maths_Status_Added);


            ViewModel = new AddMathsStatusViewModel { ProfileId = 1, LearnerName = "John Smith", IsAchieved = true };
            TrainingProviderLoader.UpdateLearnerSubjectAsync(ProviderUkprn, ViewModel).Returns(true);

            
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            TrainingProviderLoader.Received(1).UpdateLearnerSubjectAsync(ProviderUkprn, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(x => x.Message.Equals(_expectedSuccessBannerMsg)), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_LearnerRecordDetails()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.LearnerRecordDetails);
        }
    }
}
