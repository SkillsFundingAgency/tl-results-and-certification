using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using System.IO;
using System.Text;
using Xunit;
using DocumentResource = Sfa.Tl.ResultsAndCertification.Web.Content.Document;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.DocumentControllerTests.DownloadResultsDataFormatAndRulesGuide
{
    public class When_Called_With_ValidData : TestSetup
    {
        private string _fileName;
        private string _folderName;
        public override void Given()
        {
            _folderName = BlobStorageConstants.ResultsFolderName;
            _fileName = DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Results_Data_Format_And_Rules_File_Name;
            
            DocumentLoader.GetTechSpecFileAsync(_folderName, _fileName)
                .Returns(new MemoryStream(Encoding.ASCII.GetBytes("Test File for assessment entries tech spec")));
        }

        [Fact]
        public void Then_GetRegistrationValidationErrorsFileAsync_Method_Is_Called()
        {
            DocumentLoader.Received(1).GetTechSpecFileAsync(_folderName, _fileName);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            var viewResult = Result as FileStreamResult;
            viewResult.Should().NotBeNull();
            viewResult.FileDownloadName.Should().Be(DocumentResource.TlevelDataFormatAndRulesGuide.Tlevels_Results_Data_Format_And_Rules_File_Name);
            viewResult.ContentType.Should().Be("text/xlsx");
            viewResult.FileStream.Should().NotBeNull();
        }
    }
}
