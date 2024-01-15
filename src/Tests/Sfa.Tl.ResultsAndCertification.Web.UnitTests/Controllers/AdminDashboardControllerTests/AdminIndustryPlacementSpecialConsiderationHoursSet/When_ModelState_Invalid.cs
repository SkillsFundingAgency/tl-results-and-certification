using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationHoursSet
{
    public class When_ModelState_Invalid : TestSetup
    {
        private const string ErrorKey = "AdminIndustryPlacementSpecialConsiderationHours";

        public override void Given()
        {
            const int RegistrationPathwayId = 1;

            var adminChangeIpViewModel = new AdminChangeIpViewModel
            {
                AdminIpCompletion = new AdminIpCompletionViewModel
                {
                    RegistrationPathwayId = RegistrationPathwayId,
                    IndustryPlacementStatus = IndustryPlacementStatus.CompletedWithSpecialConsideration
                },
                HoursViewModel = new AdminIpSpecialConsiderationHoursViewModel
                {
                    RegistrationPathwayId = RegistrationPathwayId,
                    Hours = string.Empty
                }
            };

            ViewModel = adminChangeIpViewModel.HoursViewModel;
            Controller.ModelState.AddModelError(ErrorKey, AdminIndustryPlacementSpecialConsiderationHours.Hours_Validation_Message);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.DidNotReceive().GetAsync<AdminChangeIpViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminIpSpecialConsiderationHoursViewModel>();

            model.Should().NotBeNull();
            model.RegistrationPathwayId.Should().Be(ViewModel.RegistrationPathwayId);
            model.Hours.Should().Be(ViewModel.Hours);

            ModelStateDictionary modelState = Controller.ViewData.ModelState;
            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey(ErrorKey);
            modelState[ErrorKey].Errors[0].ErrorMessage.Should().Be(AdminIndustryPlacementSpecialConsiderationHours.Hours_Validation_Message);
        }
    }
}
