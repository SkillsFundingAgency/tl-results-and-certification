using FluentAssertions;
using FluentValidation.Results;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests
{
    public class Then_Valid_Registrations_Return_Expected_Results : When_ReadAndParseFileAsync_Is_Called
    {
        private RegistrationCsvRecordResponse expectedRow;

        public override void Given()
        {
            InputFileContent = GetInputFilecontent();

            var failures = new List<ValidationFailure>();
            expectedRow = new RegistrationCsvRecordResponse
            {
                RowNum = 1,
                Uln = 123,
                FirstName = "F name",
                LastName = "L name",
                DateOfBirth = new DateTime(2000, 1, 1),
                Ukprn = 1234,
                Core = "989",
                StartDate = new DateTime(2000, 1, 1),
                Specialisms = new List<string> { "spl1" }
            };
            var regCsvResponse = new ValidationResult(failures);
            RegValidator.ValidateAsync(Arg.Any<RegistrationCsvRecordRequest>()).Returns(regCsvResponse);
            DataParser.ParseRow(Arg.Any<RegistrationCsvRecordRequest>(), Arg.Any<int>()).Returns(expectedRow);
        }

        [Fact]
        public void Then_Expectred_Results_Are_Returned()
        {
            Response.Result.Rows.Count.Should().Be(1);
            var actualData = Response.Result.Rows.First();
            actualData.RowNum.Should().Be(expectedRow.RowNum);
            actualData.FirstName.Should().Be(expectedRow.FirstName);
            actualData.LastName.Should().Be(expectedRow.LastName);
            actualData.DateOfBirth.Should().Be(expectedRow.DateOfBirth);
            actualData.Core.Should().Be(expectedRow.Core);
            actualData.StartDate.Should().Be(expectedRow.StartDate);
            actualData.Specialisms.Count().Should().Be(expectedRow.Specialisms.Count());
            actualData.Specialisms.First().Should().Be(expectedRow.Specialisms.First());

            actualData.ValidationErrors.Count().Should().Be(0);
        }

        private StringBuilder GetInputFilecontent()
        {
            StringBuilder csvData = new StringBuilder();
            csvData.AppendLine("ULN,First Name,Last Name,Date of Birth,UKPRN,Start Date,Core code,Specialism codes");
            csvData.AppendLine("1111111111,First 1,Last 1,10012006,10000080,22092020,10423456,27234567");
            return csvData;
        }
    }
}
