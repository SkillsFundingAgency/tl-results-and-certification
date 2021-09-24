using FluentAssertions;
using FluentValidation.Results;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests.Registrations
{
    public class When_Registrations_Valid : TestSetup
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
                ProviderUkprn = 1234,
                CoreCode = "989",
                AcademicYear = 2020,
                SpecialismCodes = new List<string> { "spl1" }
            };
            var regCsvResponse = new ValidationResult(failures);
            RegValidator.ValidateAsync(Arg.Any<RegistrationCsvRecordRequest>()).Returns(regCsvResponse);
            DataParser.ParseRow(Arg.Any<RegistrationCsvRecordRequest>(), Arg.Any<int>()).Returns(expectedRow);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Response.Rows.Count.Should().Be(1);
            var actualData = Response.Rows.First();
            actualData.RowNum.Should().Be(expectedRow.RowNum);
            actualData.FirstName.Should().Be(expectedRow.FirstName);
            actualData.LastName.Should().Be(expectedRow.LastName);
            actualData.DateOfBirth.Should().Be(expectedRow.DateOfBirth);
            actualData.CoreCode.Should().Be(expectedRow.CoreCode);
            actualData.AcademicYear.Should().Be(expectedRow.AcademicYear);
            actualData.SpecialismCodes.Count().Should().Be(expectedRow.SpecialismCodes.Count());
            actualData.SpecialismCodes.First().Should().Be(expectedRow.SpecialismCodes.First());

            actualData.ValidationErrors.Count().Should().Be(0);
        }

        private StringBuilder GetInputFilecontent()
        {
            StringBuilder csvData = new StringBuilder();
            csvData.AppendLine("ULN,First Name,Last Name,Date of Birth,UKPRN,Academic year,Core code,Specialism code(s)");
            csvData.AppendLine("1111111111,First 1,Last 1,10012006,10000080,2020,10423456,27234567");
            return csvData;
        }
    }
}
