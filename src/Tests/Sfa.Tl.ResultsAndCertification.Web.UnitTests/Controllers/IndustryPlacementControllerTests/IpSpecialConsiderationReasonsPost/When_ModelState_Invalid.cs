using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpSpecialConsiderationReasonsPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new SpecialConsiderationReasonsViewModel
            {
                LearnerName = "Test User",
                AcademicYear = 2020,
                ReasonsList = new List<IpLookupDataViewModel> { new IpLookupDataViewModel { Id = 1, Name = "Medical", IsSelected = false }, new IpLookupDataViewModel { Id = 2, Name = "Withdrawn", IsSelected = false } }
            };

            Controller.ModelState.AddModelError("IsReasonSelected", Content.IndustryPlacement.IpSpecialConsiderationReasons.Validation_Message_Select_One_Or_More_Reasons);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SpecialConsiderationReasonsViewModel));

            var model = viewResult.Model as SpecialConsiderationReasonsViewModel;

            model.Should().NotBeNull();

            model.LearnerName.Should().Be(ViewModel.LearnerName);
            model.AcademicYear.Should().Be(ViewModel.AcademicYear);
            model.IsReasonSelected.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(SpecialConsiderationReasonsViewModel.IsReasonSelected)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(SpecialConsiderationReasonsViewModel.IsReasonSelected)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.IndustryPlacement.IpSpecialConsiderationReasons.Validation_Message_Select_One_Or_More_Reasons);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpSpecialConsiderationHours);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
