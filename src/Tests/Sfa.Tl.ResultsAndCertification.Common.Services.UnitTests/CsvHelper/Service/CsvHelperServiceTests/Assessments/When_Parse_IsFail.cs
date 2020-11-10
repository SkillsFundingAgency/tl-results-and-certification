using FluentAssertions;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests.Assessments
{
    public class When_Parse_IsFail : TestSetup
    {
        public override void Given()
        {
            InputFileContent = GetInputFilecontent();
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("ULN", "Uln required"),
                new ValidationFailure("Core code", "Core code required"),
            };
            var regCsvResponse = new ValidationResult(failures);
            RegValidator.ValidateAsync(Arg.Any<AssessmentCsvRecordRequest>()).Returns(regCsvResponse);
            DataParser.ParseErrorObject(Arg.Any<int>(), Arg.Any<AssessmentCsvRecordRequest>(), regCsvResponse).ReturnsNull();

        }

        [Fact]
        public void Then_Returns_Expected_Exception()
        {
            Response.Should().Throws<Exception>();
        }

        private StringBuilder GetInputFilecontent()
        {
            StringBuilder csvData = new StringBuilder();
            csvData.AppendLine("ULN,Core code,Core assessment entry,Specialism code,Specialism assessment entry");
            csvData.AppendLine("1234567890,12345678,Summer 2021,Test1234,Summer 2021");
            return csvData;
        }
    }
}
