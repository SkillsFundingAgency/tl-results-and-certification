using FluentAssertions;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests.Results
{
    public class When_Parse_IsFail : TestSetup
    {
        public override void Given()
        {
            InputFileContent = GetInputFilecontent();
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure(ResultFileHeader.Uln, string.Format(ValidationMessages.Required, ResultFluentHeader.Uln)),
            };
            var regCsvResponse = new ValidationResult(failures);
            RegValidator.ValidateAsync(Arg.Any<ResultCsvRecordRequest>()).Returns(regCsvResponse);
            DataParser.ParseErrorObject(Arg.Any<int>(), Arg.Any<ResultCsvRecordRequest>(), regCsvResponse).ReturnsNull();
        }

        [Fact]
        public void Then_Returns_Expected_Exception()
        {
            Response.Should().Throws<Exception>();
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
