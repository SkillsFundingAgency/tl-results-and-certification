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
    public class When_Extention_IsInvalid : BaseTest<FileValidationAttribute>
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
            
            var file = new FormFile(stream, 0, 0, "Data", "Registrations.xml");
            var validFile1 = new FormFile(stream, 0, 0, "Data", "reg.txt");
            var validFile2 = new FormFile(stream, 0, 0, "Data", "reg.pdf");
            var validFile3 = new FormFile(stream, 0, 0, "Data", "reg.pdf");

            model = new FileValidationModel { File = file, ValidFile1 = validFile1, ValidFile2 = validFile2, ValidFile3 = validFile3 };
            validationContext = new ValidationContext(model);
        }

        public override Task When()
        {
            overAllResult = Validator.TryValidateObject(model, validationContext, validationResults, true);
            return Task.CompletedTask;
        }

        [Fact]
        public void Then_Returns_ExpectedResults()
        {
            overAllResult.Should().BeFalse();
            validationResults.Should().NotBeNull();
            validationResults.Should().HaveCount(1);
            validationResults.First().ErrorMessage.Should().Be(ErrorResource.Upload.Must_Be_Csv_Validation_Message);
        }

        class FileValidationModel
        {
            [FileValidation(AllowedExtensions = ".csv", MaxFileNameLength = 20, ErrorResourceType = typeof(ErrorResource.Upload))]
            public IFormFile File { get; set; }

            [FileValidation(AllowedExtensions = ".csv,.txt", MaxFileNameLength = 20, ErrorResourceType = typeof(ErrorResource.Upload))]
            public IFormFile ValidFile1 { get; set; }

            [FileValidation(AllowedExtensions = ".pdf", ErrorResourceType = typeof(ErrorResource.Upload))]
            public IFormFile ValidFile2 { get; set; }

            [FileValidation(AllowedExtensions = ".pdf", MaxFileNameLength = 20, ErrorResourceType = typeof(ErrorResource.Upload))]
            public IFormFile ValidFile3 { get; set; }
        }
    }
}