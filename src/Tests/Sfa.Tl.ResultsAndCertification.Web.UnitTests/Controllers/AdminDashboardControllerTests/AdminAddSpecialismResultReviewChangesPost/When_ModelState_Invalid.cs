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

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminAddSpecialismResultReviewChangesPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private readonly AdminAddSpecialismResultReviewChangesViewModel _viewModel = CreateViewModel();
        private const string ErrorKey = "AdminAddSpecialismResultReviewChanges";

        public override void Given()
        {
            Controller.ModelState.AddModelError(ErrorKey, AdminAddSpecialismResultReviewChanges.Validation_Contact_Name_Blank_Text);
        }

        public async override Task When()
        {
            Result = await Controller.AdminAddSpecialismResultReviewChangesAsync(_viewModel);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            AdminDashboardLoader.DidNotReceive().ProcessAddSpecialismResultReviewChangesAsync(_viewModel);
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<AdminNotificationBannerModel>(), CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminAddSpecialismResultReviewChangesViewModel>();
            model.Should().BeEquivalentTo(_viewModel);

            ModelStateDictionary modelState = Controller.ViewData.ModelState;
            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey(ErrorKey);
            modelState[ErrorKey].Errors[0].ErrorMessage.Should().Be(AdminAddSpecialismResultReviewChanges.Validation_Contact_Name_Blank_Text);
        }
    }
}