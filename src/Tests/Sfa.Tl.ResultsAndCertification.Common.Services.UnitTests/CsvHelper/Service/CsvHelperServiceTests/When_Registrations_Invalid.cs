using FluentAssertions;
using FluentValidation.Results;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests
{
    public class When_Registrations_Invalid : TestSetup
    {
        private RegistrationCsvRecordResponse expectedRow;

        public override void Given()
        {
            InputFileContent = GetInputFilecontent();
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("First name", "First name required"),
            };

            expectedRow = new RegistrationCsvRecordResponse { Uln = 123, RowNum = 1,  ValidationErrors = new List<BulkProcessValidationError> 
            {
              new BulkProcessValidationError { RowNum = "1", Uln = "123", ErrorMessage = "First name required" }
            } };
            var regCsvResponse = new ValidationResult(failures);
            RegValidator.ValidateAsync(Arg.Any<RegistrationCsvRecordRequest>()).Returns(regCsvResponse);
            DataParser.ParseErrorObject(Arg.Any<int>(), Arg.Any<RegistrationCsvRecordRequest>(), regCsvResponse).Returns(expectedRow);
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
            csvData.AppendLine("ULN,First Name,Last Name,Date of Birth,UKPRN,Academic year,Core code,Specialism codes");
            csvData.AppendLine("1111111111,First 1,Last 1,10012006,10000080,2020,10423456,27234567");
            return csvData;
        }
    }
}
