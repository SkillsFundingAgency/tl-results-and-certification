using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;
using ManageSpecialismResultContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ManageSpecialismResult;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.ChangeSpecialismResultPost
{
    public class When_Success : TestSetup
    {
        private string _expectedSuccessBannerMsg;
        private ChangeResultResponse ChangeResultResponse;

        public override void Given()
        {
            ViewModel = new ManageSpecialismResultViewModel
            {
                ProfileId = 1,
                ResultId = 1,
                Uln = 1234567890,
                SelectedGradeCode = "SCG1",
                AssessmentSeries = "summer 2022",
                SpecialismName = "Specialism"
            };

            ChangeResultResponse = new ChangeResultResponse
            {
                IsSuccess = true,
                Uln = 1234567890,
                ProfileId = 1
            };

            _expectedSuccessBannerMsg = string.Format(ManageSpecialismResultContent.Banner_Message_For_Result_Changed, ViewModel.AssessmentSeries, ViewModel.SpecialismName);

            ResultLoader.IsSpecialismResultChangedAsync(AoUkprn, ViewModel).Returns(true);
            ResultLoader.ChangeSpecialismResultAsync(AoUkprn, ViewModel).Returns(ChangeResultResponse);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).IsSpecialismResultChangedAsync(AoUkprn, ViewModel);
            ResultLoader.Received(1).ChangeSpecialismResultAsync(AoUkprn, ViewModel);
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
