using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests.Assessments
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
            csvData.AppendLine("ULN,InvalidColumn,Core assessment entry,Specialism code,Specialism assessment entry");
            csvData.AppendLine("1234567890,123456789,Summer 2021,Test1234,Summer 2021");
            return csvData;
        }
    }
}
