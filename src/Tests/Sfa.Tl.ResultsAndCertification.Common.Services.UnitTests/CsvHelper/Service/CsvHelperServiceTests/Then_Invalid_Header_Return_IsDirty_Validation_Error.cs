using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests
{
    public class Then_Invalid_Header_Return_IsDirty_Validation_Error : When_ReadAndParseFileAsync_Is_Called
    {
        public override void Given()
        {
            InputFileContent = GetInputFilecontent();
        }

        [Fact]
        public void Then_Returns_Header_Not_Found_Error()
        {
            var actualResult = Response.Result;
            actualResult.IsDirty.Should().BeTrue();
            actualResult.Rows.Count().Should().Be(0);
            actualResult.ErrorMessage.Should().Be(ValidationMessages.FileHeaderNotFound);
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
