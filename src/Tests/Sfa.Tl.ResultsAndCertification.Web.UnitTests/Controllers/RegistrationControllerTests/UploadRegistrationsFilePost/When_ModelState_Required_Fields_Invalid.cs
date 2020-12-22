using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Web.Content.Registration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using Xunit;
using UploadContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.Upload;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UploadRegistrationsFilePost
{
    public class When_ModelState_Required_Fields_Invalid : TestSetup
    {
        public override void Given()
        {
            Controller.ModelState.AddModelError("File", UploadContent.Select_File_To_Upload_Required_Validation_Message);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(UploadRegistrationsRequestViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(UploadRegistrationsRequestViewModel.File)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(UploadRegistrationsRequestViewModel.File)];
            modelState.Errors[0].ErrorMessage.Should().Be(UploadContent.Select_File_To_Upload_Required_Validation_Message);
        }
    }
}
