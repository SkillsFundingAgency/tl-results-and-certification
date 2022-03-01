using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Linq;
using System.Text;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.UnitTests.CsvHelper.Service.CsvHelperServiceTests.Results
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
            csvData.AppendLine("DIRTYColumn,ComponentCode (Core),AssessmentSeries (Core),ComponentGrade (Core),ComponentCode (Specialisms),AssessmentSeries (Specialisms),ComponentGrade (Specialisms)");
            return csvData;
        }
    }
}
