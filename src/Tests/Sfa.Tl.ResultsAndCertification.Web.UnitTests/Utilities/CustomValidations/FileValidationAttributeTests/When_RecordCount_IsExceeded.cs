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
    public class When_RecordCount_IsExceeded : BaseTest<FileValidationAttribute>
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
            var moreDataStream = new MemoryStream(Encoding.UTF8.GetBytes("Header \nrec1, \nrec2, \nrec3, \nrec4"));
            var validDataStream = new MemoryStream(Encoding.UTF8.GetBytes("Header \nrec1, \nrec2, \nrec3"));

            var file = new FormFile(moreDataStream, 0, 256, "Data", "Registrations.csv");
            var validFile = new FormFile(validDataStream, 0, 256, "Data", "reg.csv");

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
            validationResults.First().ErrorMessage.Should().Be(string.Format(ErrorResource.Upload.File_Max_Record_Count_Validation_Message, 3));
        }

        class FileValidationModel
        {
            [FileValidation(MaxRecordCount = 3, ErrorResourceType = typeof(ErrorResource.Upload))]
            public IFormFile File { get; set; }

            [FileValidation(MaxRecordCount = 3, ErrorResourceType = typeof(ErrorResource.Upload))]
            public IFormFile ValidFile { get; set; }
        }
    }
}