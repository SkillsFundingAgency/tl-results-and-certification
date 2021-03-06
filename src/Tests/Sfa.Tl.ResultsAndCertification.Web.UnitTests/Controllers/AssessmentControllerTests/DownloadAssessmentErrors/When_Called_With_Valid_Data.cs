﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Xunit;
using AssessmentContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.AssessmentControllerTests.DownloadAssessmentErrors
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        public override void Given()
        {
            Id = BlobUniqueReference.ToString();
        }

        [Fact]
        public void Then_Expected_Methods_Called()
        {
            AssessmentLoader.Received(1).GetAssessmentValidationErrorsFileAsync(Ukprn, Id.ToGuid());
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as FileStreamResult;
            viewResult.Should().NotBeNull();
            viewResult.FileDownloadName.Should().Be(AssessmentContent.UploadUnsuccessful.Assessment_Error_Report_File_Name_Text);
            viewResult.ContentType.Should().Be("text/csv");
            viewResult.FileStream.Should().NotBeNull();
        }
    }
}
