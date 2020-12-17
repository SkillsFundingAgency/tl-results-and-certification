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
    public class When_Filesize_IsExceeded : BaseTest<FileValidationAttribute>
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
            var filelength = 2 * 1024 * 1024; // 2mb

            var file = new FormFile(stream, 0, filelength, "Data", "Registrations.xml");
            var validFile = new FormFile(stream, 0, 0, "Data", "reg.txt");

            model = new FileValidationModel { File = file, ValidFile = validFile };
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
            validationResults.First().ErrorMessage.Should().Be(string.Format(ErrorResource.Upload.File_Size_Too_Large_Validation_Message, 1));
        }

        class FileValidationModel
        {
            [FileValidation(MaxFileSizeInMb = 1, ErrorResourceType = typeof(ErrorResource.Upload))]
            public IFormFile File { get; set; }

            [FileValidation(MaxFileSizeInMb = 1, AllowedExtensions = ".csv,.txt", MaxFileNameLength = 20, ErrorResourceType = typeof(ErrorResource.Upload))]
            public IFormFile ValidFile { get; set; }
        }
    }
}