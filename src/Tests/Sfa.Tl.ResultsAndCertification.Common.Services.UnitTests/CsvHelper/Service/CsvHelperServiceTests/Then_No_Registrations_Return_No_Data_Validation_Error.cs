using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests
{
    public class Then_No_Registrations_Return_No_Data_Validation_Error : When_ReadAndParseFileAsync_Is_Called
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
            actualResult.ErrorMessage.Should().Be(ValidationMessages.NoRecordsFound);
        }

        private StringBuilder GetInputFilecontent()
        {
            StringBuilder csvData = new StringBuilder();
            csvData.AppendLine("ULN,First Name,Last Name,Date of Birth,UKPRN,Registration Date,Core code,Specialism codes");
            return csvData;
        }
    }
}
