using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AdminDashboardControllerTests.AdminIndustryPlacementSpecialConsiderationReasonsPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private const string ErrorKey = "AdminIndustryPlacementSpecialConsiderationReasons";

        public override void Given()
        {
            ViewModel = new AdminIpSpecialConsiderationReasonsViewModel
            {
                RegistrationPathwayId = 157,
                ReasonsList = new List<IpLookupDataViewModel>
                    {
                        new IpLookupDataViewModel
                        {
                            Id = 1,
                            Name = "Domestic crisis",
                            IsSelected = false
                        }
                    }
            };

            Controller.ModelState.AddModelError(ErrorKey, AdminIndustryPlacementSpecialConsiderationReasons.Validation_Message_Select_One_Or_More_Reasons);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            CacheService.DidNotReceive().GetAsync<AdminChangeIpViewModel>(CacheKey);
            CacheService.DidNotReceive().SetAsync(CacheKey, Arg.Any<AdminChangeIpViewModel>());
        }

        [Fact]
        public void Then_Returns_Expected()
        {
            var model = Result.ShouldBeViewResult<AdminIpSpecialConsiderationReasonsViewModel>();

            model.Should().NotBeNull();
            model.RegistrationPathwayId.Should().Be(ViewModel.RegistrationPathwayId);
            model.ReasonsList.Should().BeEquivalentTo(ViewModel.ReasonsList);

            ModelStateDictionary modelState = Controller.ViewData.ModelState;
            modelState.Should().HaveCount(1);
            modelState.Should().ContainKey(ErrorKey);
            modelState[ErrorKey].Errors[0].ErrorMessage.Should().Be(AdminIndustryPlacementSpecialConsiderationReasons.Validation_Message_Select_One_Or_More_Reasons);
        }
    }
}
