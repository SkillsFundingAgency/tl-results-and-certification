using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Result;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminChangeSpecialismResultReviewChangesPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private readonly AdminChangeSpecialismResultReviewChangesViewModel _viewModel = CreateViewModel();
        private const string ErrorKey = "AdminChangeSpecialismResultReviewChanges";

        public override void Given()
        {
            Controller.ModelState.AddModelError(ErrorKey, AdminChangeSpecialismResultReviewChanges.Validation_Contact_Name_Blank_Text);
        }

        public async override Task When()
        {
            Result = await Controller.AdminChangeSpecialismResultReviewChangesAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.DidNotReceive().ProcessChangeSpecialismResultReviewChangesAsync(_viewModel);
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<AdminNotificationBannerModel>(), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminChangeSpecialismResultReviewChangesViewModel>();
            model.Should().BeEquivalentTo(_viewModel);

            ModelStateDictionary modelState = Controller.ViewData.ModelState;
            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey(ErrorKey);
            modelState[ErrorKey].Errors[0].ErrorMessage.Should().Be(AdminChangeSpecialismResultReviewChanges.Validation_Contact_Name_Blank_Text);
        }
    }
}