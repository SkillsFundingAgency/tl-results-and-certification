using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;
using SelectSpecialismContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.SelectSpecialisms;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationSpecialismPost
{
    public class Then_On_ModelState_Invalid_Returns_Required_Validation_Error : When_AddRegistrationSpecialism_Action_Is_Called
    {
        public override void Given()
        {
            SelectSpecialismViewModel = new SelectSpecialismViewModel();
            Controller.ModelState.AddModelError("HasSpecialismSelected", SelectSpecialismContent.Validation_Select_Specialism_Required_Message);
        }

        [Fact]
        public void Then_Expected_Required_Error_Message_Is_Returned()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SelectSpecialismViewModel));

            Controller.ViewData.ModelState.ContainsKey(nameof(SelectSpecialismViewModel.HasSpecialismSelected)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(SelectSpecialismViewModel.HasSpecialismSelected)];
            modelState.Errors[0].ErrorMessage.Should().Be(SelectSpecialismContent.Validation_Select_Specialism_Required_Message);
        }
    }
}
