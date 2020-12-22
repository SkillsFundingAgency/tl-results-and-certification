using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests.Registrations
{
    public class When_Header_Invalid : TestSetup
    {
        public override void Given()
        {
            InputFileContent = GetInputFilecontent();
        }

        [Fact]
        public void Then_Returns_Error_FileHeaderNotFound()
        {
            var actualResult = Response;
            actualResult.IsDirty.Should().BeTrue();
            actualResult.Rows.Count().Should().Be(0);
            actualResult.ErrorMessage.Should().Be(ValidationMessages.FileHeaderNotFound);
            actualResult.ErrorCode.Should().Be(CsvFileErrorCode.HeaderInvalid);
        }

        private StringBuilder GetInputFilecontent()
        {
            StringBuilder csvData = new StringBuilder();
            csvData.AppendLine("Test,Invalid Name,Last Name,Date of Birth,UKPRN,Academic year,Core code,Specialism codes");
            csvData.AppendLine("1111111111,First 1,Last 1,10012006,10000080,2020,10423456,27234567");
            return csvData;
        }
    }
}
