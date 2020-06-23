using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using Xunit;
using UploadContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.Upload;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UploadRegistrationsFilePost
{
    public class Then_On_ModelState_Invalid_Returns_MaxRecord_Validation_Error : When_UploadRegistrationsFile_Post_Action_Is_Called
    {
        public override void Given()
        {
            FormFile = Substitute.For<IFormFile>();
            Controller.ModelState.AddModelError("File", string.Format(UploadContent.File_Max_Record_Count_Validation_Message, 10000));
        }

        [Fact]
        public void Then_Expected_Must_Be_MaxRecord_Length_Error_Message_Is_Returned()
        {
            Result.Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result.Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(UploadRegistrationsRequestViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(UploadRegistrationsRequestViewModel.File)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(UploadRegistrationsRequestViewModel.File)];
            modelState.Errors[0].ErrorMessage.Should().Be(string.Format(UploadContent.File_Max_Record_Count_Validation_Message, 10000));
        }
    }
}
