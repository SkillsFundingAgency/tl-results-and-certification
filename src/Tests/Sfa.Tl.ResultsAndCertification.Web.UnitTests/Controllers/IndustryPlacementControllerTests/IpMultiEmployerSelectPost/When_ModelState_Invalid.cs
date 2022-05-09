using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpMultiEmployerSelectPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        public override void Given()
        {
            ViewModel = new IpMultiEmployerSelectViewModel
            {
                LearnerName = "Test User",
                PlacementModels = new List<IpLookupDataViewModel> { new IpLookupDataViewModel { Id = 1, Name = "Placement 1", IsSelected = false }, new IpLookupDataViewModel { Id = 2, Name = "Placement 2", IsSelected = false } }
            };

            Controller.ModelState.AddModelError("IsIpModelSelected", Content.IndustryPlacement.IpMultiEmployerSelect.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IpMultiEmployerSelectViewModel));

            var model = viewResult.Model as IpMultiEmployerSelectViewModel;

            model.Should().NotBeNull();

            model.LearnerName.Should().Be(ViewModel.LearnerName);            
            model.IsIpModelSelected.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(IpMultiEmployerSelectViewModel.IsIpModelSelected)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(IpMultiEmployerSelectViewModel.IsIpModelSelected)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.IndustryPlacement.IpMultiEmployerSelect.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpMultiEmployerUsed);
            model.BackLink.RouteAttributes.Count.Should().Be(0);
        }
    }
}
