using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;
using SelectSpecialisms = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SelectSpecialisms;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ChangeSpecialismsPost
{
    public class When_ModelState_Invalid : TestSetup
    {
        private ChangeSpecialismViewModel mockChangeSpecialismViewModel = null;

        public override void Given()
        {
            ViewModel.PathwaySpecialisms = new ViewModel.PathwaySpecialismsViewModel();
            mockChangeSpecialismViewModel = new ChangeSpecialismViewModel { SpecialismCodes = new List<string>()};
            RegistrationLoader.GetRegistrationProfileAsync<ChangeSpecialismViewModel>(AoUkprn, ViewModel.ProfileId).Returns(mockChangeSpecialismViewModel);
            Controller.ModelState.AddModelError("HasSpecialismSelected", SelectSpecialisms.Validation_Select_Specialism_Required_Message);
        }

        [Fact]
        public void Then_Returns_Expected_ViewModel()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ChangeSpecialismViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(ChangeSpecialismViewModel.HasSpecialismSelected)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(ChangeSpecialismViewModel.HasSpecialismSelected)];
            modelState.Errors[0].ErrorMessage.Should().Be(SelectSpecialisms.Validation_Select_Specialism_Required_Message);
        }
    }
}
