using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;
using ManageCoreResultContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ManageCoreResult;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.AddCoreResultPost
{
    public class When_Success : TestSetup
    {
        private string _expectedSuccessBannerMsg;
        private AddResultResponse AddResultResponse;

        public override void Given()
        {
            ViewModel = new ManageCoreResultViewModel
            {
                ProfileId = 1,
                SelectedGradeCode = "PCG1",
                AssessmentSeries = "summer 2021",
                PathwayName = "Test pathway"
            };

            AddResultResponse = new AddResultResponse
            {
                IsSuccess = true,
                Uln = 1234567890,
                ProfileId = 1
            };

            _expectedSuccessBannerMsg = string.Format(ManageCoreResultContent.Banner_Message_For_Result_Added, ViewModel.AssessmentSeries, ViewModel.PathwayName);

            ResultLoader.AddCoreResultAsync(AoUkprn, ViewModel).Returns(AddResultResponse);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).AddCoreResultAsync(AoUkprn, ViewModel);
            CacheService.Received(1).SetAsync(CacheKey, Arg.Is<NotificationBannerModel>(x => x.Message.Equals(_expectedSuccessBannerMsg)), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_ResultDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.ResultDetails);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}
