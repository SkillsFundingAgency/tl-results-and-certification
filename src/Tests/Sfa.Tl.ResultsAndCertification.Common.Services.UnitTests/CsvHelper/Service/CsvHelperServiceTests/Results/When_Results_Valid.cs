using FluentAssertions;
using FluentValidation.Results;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests.Results
{
    public class When_Results_Valid : TestSetup
    {
        private ResultCsvRecordResponse expectedRow;

        public override void Given()
        {
            InputFileContent = GetInputFilecontent();

            var failures = new List<ValidationFailure>();
            expectedRow = new ResultCsvRecordResponse
            {
                RowNum = 1,
                Uln = 1234567890,
                CoreCode = "12345678",
                CoreAssessmentSeries = "Summer 2021",
                CoreGrade = "A",
            };
            var regCsvResponse = new ValidationResult(failures);
            RegValidator.ValidateAsync(Arg.Any<ResultCsvRecordRequest>()).Returns(regCsvResponse);
            DataParser.ParseRow(Arg.Any<ResultCsvRecordRequest>(), Arg.Any<int>()).Returns(expectedRow);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Response.Rows.Count.Should().Be(1);
            var actualData = Response.Rows.First();
            actualData.RowNum.Should().Be(expectedRow.RowNum);
            actualData.Uln.Should().Be(expectedRow.Uln);
            actualData.CoreCode.Should().Be(expectedRow.CoreCode);
            actualData.CoreAssessmentSeries.Should().Be(expectedRow.CoreAssessmentSeries);
            actualData.CoreGrade.Should().Be(expectedRow.CoreGrade);
            actualData.ValidationErrors.Count().Should().Be(0);
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
