﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.PostResultsServiceControllerTests.UploadRommsUnsuccessful
{
    public class When_TempData_Found : TestSetup
    {
        public override void Given()
        {
            BlobUniqueReference = Guid.NewGuid();
            UploadUnsuccessfulViewModel = new UploadUnsuccessfulViewModel { BlobUniqueReference = BlobUniqueReference, FileSize = 1.7, FileType = FileType.Csv.ToString().ToUpperInvariant() };
            CacheService.GetAndRemoveAsync<UploadUnsuccessfulViewModel>(Arg.Any<string>()).Returns(UploadUnsuccessfulViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as ViewResult;
            var model = viewResult.Model as UploadUnsuccessfulViewModel;

            model.Should().NotBeNull();
            model.BlobUniqueReference.Should().Be(UploadUnsuccessfulViewModel.BlobUniqueReference);
            model.FileSize.Should().Be(UploadUnsuccessfulViewModel.FileSize);
            model.FileType.Should().Be(UploadUnsuccessfulViewModel.FileType);
        }
    }
}
