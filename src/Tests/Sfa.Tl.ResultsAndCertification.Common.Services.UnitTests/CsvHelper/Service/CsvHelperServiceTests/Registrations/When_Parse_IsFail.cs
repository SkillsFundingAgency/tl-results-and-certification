using FluentAssertions;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests.Registrations
{
    public class When_Parse_IsFail : TestSetup
    {
        public override void Given()
        {
            InputFileContent = GetInputFilecontent();
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("ULN", "Uln required"),
                new ValidationFailure("First name", "First name required"),
            };
            var regCsvResponse = new ValidationResult(failures);
            RegValidator.ValidateAsync(Arg.Any<RegistrationCsvRecordRequest>()).Returns(regCsvResponse);
            DataParser.ParseErrorObject(Arg.Any<int>(), Arg.Any<RegistrationCsvRecordRequest>(), regCsvResponse).ReturnsNull();

        }

        [Fact]
        public void Then_Returns_Expected_Exception()
        {
            Response.Should().Throws<Exception>();
        }

        private StringBuilder GetInputFilecontent()
        {
            StringBuilder csvData = new StringBuilder();
            csvData.AppendLine("ULN,First Name,Last Name,Date of Birth,UKPRN,Academic Year,Core code,Specialism code(s)");
            csvData.AppendLine("1111111111,First 1,Last 1,10012006,10000080,2020,10423456,27234567");
            return csvData;
        }
    }
}
