using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DocumentControllerTests.DownloadRegistrationDataFormatAndRulesGuide;
using System.IO;
using System.Text;
using Xunit;
using DocumentResource = Sfa.Tl.ResultsAndCertification.Web.Content.Document;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DocumentControllerTests.DownloadRegistrationDataFormatAndRulesGuideGet
{
    public class When_Action_Called : TestSetup
    {
        private string _fileName;
        public override void Given()
        {
            _fileName = DocumentResource.TlevelDataFormatAndRulesGuide.Registrations_Data_Format_And_Rules_Guide_File_Name_Text;
            DocumentLoader.GetBulkUploadRegistrationsTechSpecFileAsync(_fileName).Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File for registration tech spec")));
        }

        [Fact]
        public void Then_GetRegistrationValidationErrorsFileAsync_Method_Is_Called()
        {
            DocumentLoader.Received(1).GetBulkUploadRegistrationsTechSpecFileAsync(_fileName);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as FileStreamResult;
            viewResult.Should().NotBeNull();
            viewResult.FileDownloadName.Should().Be(DocumentResource.TlevelDataFormatAndRulesGuide.Registrations_Data_Format_And_Rules_Guide_File_Name_Text);
            viewResult.ContentType.Should().Be("text/xlsx");
            viewResult.FileStream.Should().NotBeNull();
        }
    }
}
