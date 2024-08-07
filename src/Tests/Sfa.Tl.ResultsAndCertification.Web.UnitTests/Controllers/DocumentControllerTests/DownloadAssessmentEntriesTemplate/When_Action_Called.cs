﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.IO;
using System.Text;
using Xunit;
using DocumentResource = Sfa.Tl.ResultsAndCertification.Web.Content.Document;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DocumentControllerTests.DownloadAssessmentEntriesTemplate
{
    public class When_Action_Called : TestSetup
    {
        private string _fileName;
        public override void Given()
        {
            _fileName = DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Assessment_Entry_Data_Template_File_Name;
            DocumentLoader.GetBulkUploadAssessmentEntriesTechSpecFileAsync(_fileName).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File for registration tech spec")));
        }

        [Fact]
        public void Then_GetRegistrationValidationErrorsFileAsync_Method_Is_Called()
        {
            DocumentLoader.Received(1).GetBulkUploadAssessmentEntriesTechSpecFileAsync(_fileName);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as FileStreamResult;
            viewResult.Should().NotBeNull();
            viewResult.FileDownloadName.Should().Be(DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Assessment_Entry_Data_Template_File_Name);
            viewResult.ContentType.Should().Be("text/csv");
            viewResult.FileStream.Should().NotBeNull();
        }
    }
}
