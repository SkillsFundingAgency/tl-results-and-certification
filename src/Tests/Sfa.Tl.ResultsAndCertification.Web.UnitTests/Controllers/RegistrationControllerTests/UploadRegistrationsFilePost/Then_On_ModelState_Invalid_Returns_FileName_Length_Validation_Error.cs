using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System;
using System.Linq;
using Xunit;
using UploadContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.Upload;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UploadRegistrationsFilePost
{
    public class Then_On_ModelState_Invalid_Returns_FileName_Length_Validation_Error : When_UploadRegistrationsFile_Post_Action_Is_Called
    {
        public override void Given()
        {
            var _random = new Random();
            var randomString = new String(Enumerable.Range(0, 300).Select(n => (Char)(_random.Next(97, 122))).ToArray());
            var filename = $"{randomString}.csv";
            FormFile = Substitute.For<IFormFile>();
            FormFile.FileName.Returns(filename);
            Controller.ModelState.AddModelError("File", UploadContent.File_Name_Length_Validation_Message);
        }

        [Fact]
        public void Then_Expected_Must_Be_FileName_Length_Error_Message_Is_Returned()
        {
            Result.Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result.Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(UploadRegistrationsRequestViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(UploadRegistrationsRequestViewModel.File)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(UploadRegistrationsRequestViewModel.File)];
            modelState.Errors[0].ErrorMessage.Should().Be(UploadContent.File_Name_Length_Validation_Message);
        }
    }
}
