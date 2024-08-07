﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System;
using System.Linq;
using Xunit;
using UploadContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.Upload;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UploadWithdrawlsFilePost
{
    public class When_ModelState_FileName_Invalid : TestSetup
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
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(UploadWithdrawalsRequestViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(UploadWithdrawalsRequestViewModel.File)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(UploadWithdrawalsRequestViewModel.File)];
            modelState.Errors[0].ErrorMessage.Should().Be(UploadContent.File_Name_Length_Validation_Message);
        }
    }
}
