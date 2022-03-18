using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System;
using Xunit;
using PrsAddRommOutcomeKnownContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAddRommOutcomeKnown;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.PrsAddRommOutcomeKnownPost
{
    public class When_RommOutcome_No_IsSuccess_For_Specialism : TestSetup
    {
        private readonly bool _prsActivityResponse = true;
        private string _expectedSuccessBannerMsg;

        public override void Given()
        {
            ComponentType = ComponentType.Specialism;
            ViewModel = new PrsAddRommOutcomeKnownViewModel
            {
                ProfileId = 1,
                AssessmentId = 11,
                ResultId = 17,
                Firstname = "Test",
                Lastname = "John",
                ExamPeriod = "Summer 2022",
                SpecialismDisplayName = "Education (1234567)",
                ComponentType = ComponentType,
                RommOutcome = RommOutcomeKnownType.No,
                RommEndDate = DateTime.Today.AddDays(7)
            };
            Loader.GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType)
                .Returns(ViewModel);

            Loader.PrsRommActivityAsync(AoUkprn, ViewModel).Returns(_prsActivityResponse);
            _expectedSuccessBannerMsg = string.Format(PrsAddRommOutcomeKnownContent.Banner_Message, ViewModel.LearnerName, ViewModel.ExamPeriod, ViewModel.CoreDisplayName);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            Loader.Received(1).GetPrsLearnerDetailsAsync<PrsAddRommOutcomeKnownViewModel>(AoUkprn, ViewModel.ProfileId, ViewModel.AssessmentId, ComponentType);
            Loader.Received(1).PrsRommActivityAsync(AoUkprn, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(x => x.Message.Equals(_expectedSuccessBannerMsg)), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_PrsLearnerDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.PrsLearnerDetails);
            route.RouteValues.Count.Should().Be(1);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
