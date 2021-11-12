using FluentAssertions;
using FluentValidation.Results;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests.Assessments
{
    public class When_Assessments_Valid : TestSetup
    {
        private AssessmentCsvRecordResponse expectedRow;

        public override void Given()
        {
            InputFileContent = GetInputFilecontent();

            var failures = new List<ValidationFailure>();
            expectedRow = new AssessmentCsvRecordResponse
            {
                RowNum = 1,
                Uln = 1234567890,
                CoreCode = "12345678",
                SpecialismCode = "Test1234",
                CoreAssessmentEntry = "Summer 2021",
                SpecialismAssessmentEntry = "Autumn 2021"
            };
            var regCsvResponse = new ValidationResult(failures);
            RegValidator.ValidateAsync(Arg.Any<AssessmentCsvRecordRequest>()).Returns(regCsvResponse);
            DataParser.ParseRow(Arg.Any<AssessmentCsvRecordRequest>(), Arg.Any<int>()).Returns(expectedRow);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Response.Rows.Count.Should().Be(1);
            var actualData = Response.Rows.First();
            actualData.RowNum.Should().Be(expectedRow.RowNum);
            actualData.Uln.Should().Be(expectedRow.Uln);
            actualData.CoreCode.Should().Be(expectedRow.CoreCode);
            actualData.CoreAssessmentEntry.Should().Be(expectedRow.CoreAssessmentEntry);
            actualData.SpecialismCode.Should().Be(expectedRow.SpecialismCode);
            actualData.SpecialismAssessmentEntry.Should().Be(expectedRow.SpecialismAssessmentEntry);
            actualData.ValidationErrors.Count().Should().Be(0);
        }

        private StringBuilder GetInputFilecontent()
        {
            StringBuilder csvData = new StringBuilder();
            csvData.AppendLine(AssessmentEntryHeader);
            csvData.AppendLine("1234567890,12345678,Summer 2021,Test1234,Autumn 2021");
            return csvData;
        }
    }
}
