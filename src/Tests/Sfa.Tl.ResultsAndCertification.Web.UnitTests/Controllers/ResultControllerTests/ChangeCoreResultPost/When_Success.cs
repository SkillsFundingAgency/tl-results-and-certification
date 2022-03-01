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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ChangeCoreResultPost
{
    public class When_Success : TestSetup
    {
        private string _expectedSuccessBannerMsg;
        private ChangeResultResponse ChangeResultResponse;

        public override void Given()
        {
            ViewModel = new ManageCoreResultViewModel
            {
                ProfileId = 1,
                ResultId = 1,
                Uln = 1234567890,
                SelectedGradeCode = "PCG1"
            };

            ChangeResultResponse = new ChangeResultResponse
            {
                IsSuccess = true,
                Uln = 1234567890,
                ProfileId = 1
            };

            _expectedSuccessBannerMsg = string.Format(ManageCoreResultContent.Banner_Message_For_Result_Changed, ViewModel.AssessmentSeries, ViewModel.PathwayName);

            ResultLoader.IsCoreResultChangedAsync(AoUkprn, ViewModel).Returns(true);
            ResultLoader.ChangeCoreResultAsync(AoUkprn, ViewModel).Returns(ChangeResultResponse);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).IsCoreResultChangedAsync(AoUkprn, ViewModel);
            ResultLoader.Received(1).ChangeCoreResultAsync(AoUkprn, ViewModel);
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
