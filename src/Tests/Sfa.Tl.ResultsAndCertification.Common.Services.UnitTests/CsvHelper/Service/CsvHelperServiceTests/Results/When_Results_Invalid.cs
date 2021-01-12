using FluentAssertions;
using FluentValidation.Results;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests.Results
{
    public class When_Results_Invalid : TestSetup
    {
        private ResultCsvRecordResponse expectedRow;

        public override void Given()
        {
            InputFileContent = GetInputFilecontent();
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Core code", "Core code must have 8 digits only"),
            };

            expectedRow = new ResultCsvRecordResponse
            {
                Uln = 123,
                RowNum = 1,
                ValidationErrors = new List<BulkProcessValidationError>
            {
              new BulkProcessValidationError { RowNum = "1", Uln = "123", ErrorMessage = "Core code must have 8 digits only" }
            }
            };
            var regCsvResponse = new ValidationResult(failures);
            RegValidator.ValidateAsync(Arg.Any<ResultCsvRecordRequest>()).Returns(regCsvResponse);
            DataParser.ParseErrorObject(Arg.Any<int>(), Arg.Any<ResultCsvRecordRequest>(), regCsvResponse).Returns(expectedRow);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Response.Rows.Count.Should().Be(1);
            var actualData = Response.Rows.First();
            actualData.Uln.Should().Be(expectedRow.Uln);
            actualData.ValidationErrors.Count().Should().Be(1);

            var actualError = actualData.ValidationErrors.First();
            var expectedError = expectedRow.ValidationErrors.First();

            actualError.RowNum.Should().Be(expectedError.RowNum);
            actualError.Uln.Should().Be(expectedError.Uln);
            actualError.ErrorMessage.Should().Be(expectedError.ErrorMessage);
        }

        private StringBuilder GetInputFilecontent()
        {
            StringBuilder csvData = new StringBuilder();
            csvData.AppendLine("ULN,ComponentCode (Core),AssessmentSeries (Core),ComponentGrade (Core)");
            csvData.AppendLine("1234567890,12345678,Summer 2021,A");
            return csvData;
        }
    }
}
