using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementControllerTests.IpMultiEmployerUsedPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private IpMultiEmployerUsedViewModel _ipMultiEmployerUsedViewModel;

        public override void Given()
        {
            _ipMultiEmployerUsedViewModel = new IpMultiEmployerUsedViewModel
            {
                LearnerName = "John Smith",
                IsMultiEmployerModelUsed = null
            };

            viewModel = new IpMultiEmployerUsedViewModel { LearnerName = "John Smith", IsMultiEmployerModelUsed = null };
            Controller.ModelState.AddModelError("IsMultiEmployerModelUsed", Content.IndustryPlacement.IpMultiEmployerUsed.Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(IpMultiEmployerUsedViewModel));

            var model = viewResult.Model as IpMultiEmployerUsedViewModel;

            model.Should().NotBeNull();

            model.LearnerName.Should().Be(viewModel.LearnerName);
            model.IsMultiEmployerModelUsed.Should().BeNull();

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(IpMultiEmployerUsedViewModel.IsMultiEmployerModelUsed)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(IpMultiEmployerUsedViewModel.IsMultiEmployerModelUsed)];
            modelState.Errors[0].ErrorMessage.Should().Be(Content.IndustryPlacement.IpMultiEmployerUsed.Validation_Message);

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.IpModelUsed);
            model.BackLink.RouteAttributes.Count.Should().Be(0);            
        }
    }
}
