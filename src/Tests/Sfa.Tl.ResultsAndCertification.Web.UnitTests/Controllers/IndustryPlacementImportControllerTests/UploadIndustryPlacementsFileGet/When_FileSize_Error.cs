﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement;
using Xunit;
using UploadContent = Sfa.Tl.ResultsAndCertification.Web.Content.IndustryPlacement.Upload;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.IndustryPlacementImportControllerTests.UploadIndustryPlacementsFileGet
{
    public class When_FileSize_Error : TestSetup
    {
        public override void Given()
        {
            RequestErrorTypeId = (int)RequestErrorType.FileSize;
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(UploadIndustryPlacementsRequestViewModel));

            Controller.ViewData.ModelState.Should().ContainSingle();
            Controller.ViewData.ModelState.ContainsKey(nameof(UploadIndustryPlacementsRequestViewModel.File)).Should().BeTrue();

            var modelState = Controller.ViewData.ModelState[nameof(UploadIndustryPlacementsRequestViewModel.File)];
            modelState.Errors[0].ErrorMessage.Should().Be(string.Format(UploadContent.File_Size_Too_Large_Validation_Message, 2));
        }
    }
}
