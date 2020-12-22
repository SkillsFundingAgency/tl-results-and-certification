using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Sfa.Tl.ResultsAndCertification.Tests.Common.BaseTest;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Utilities.CustomValidations.FileValidationAttributeTests
{
    public class When_Filename_Length_IsExceeded : BaseTest<FileValidationAttribute>
    {
        private FileValidationModel model;
        private ValidationContext validationContext;
        private List<ValidationResult> validationResults;
        private bool overAllResult;

        public override void Setup()
        {
            validationResults = new List<ValidationResult>();
        }
        
        public override void Given() 
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("This is a test file"));
            var file = new FormFile(stream, 0, 0, "Data", "File_With_Longer_Name.xml");
            var validFile = new FormFile(stream, 0, 0, "Data", "reg.csv");

            model = new FileValidationModel { File = file, ValidFile = validFile };
            validationContext = new ValidationContext(model);
        }

        public override Task When()
        {
            overAllResult =  Validator.TryValidateObject(model, validationContext, validationResults, true);
            return Task.CompletedTask;
        }
        
        [Fact]
        public void Then_Returns_ExpectedResults()
        {
            overAllResult.Should().BeFalse();
            validationResults.Should().NotBeNull();
            validationResults.Should().HaveCount(1);
            validationResults.First().ErrorMessage.Should().Be(string.Format(ErrorResource.Upload.File_Name_Length_Validation_Message, 10));
        }

        class FileValidationModel
        {
            [FileValidation(MaxFileNameLength = 10, ErrorResourceType = typeof(ErrorResource.Upload))]
            public IFormFile File { get; set; }

            [FileValidation(MaxFileNameLength = 10, ErrorResourceType = typeof(ErrorResource.Upload))]
            public IFormFile ValidFile { get; set; }
        }
    }
}