﻿using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.UploadRegistrationsFilePost
{
    public class When_Success : TestSetup
    {
        public override void Given()
        {
            FormFile = Substitute.For<IFormFile>();
            FormFile.FileName.Returns("test.csv");
            ViewModel.File = FormFile;

            ResponseViewModel = new UploadRegistrationsResponseViewModel
            {
                IsSuccess = true
            };

            RegistrationLoader.ProcessBulkRegistrationsAsync(ViewModel).Returns(ResponseViewModel);
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            RegistrationLoader.Received(1).ProcessBulkRegistrationsAsync(ViewModel);
        }

        [Fact]
        public void Then_Redirected_To_RegistrationsUploadSuccessful()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.RegistrationsUploadSuccessful);
        }
    }
}
