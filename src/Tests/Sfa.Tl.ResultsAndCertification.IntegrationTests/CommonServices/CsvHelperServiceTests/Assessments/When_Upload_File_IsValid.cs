using FluentAssertions;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Linq;
using Xunit;
using System.IO;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices.CsvHelperServiceTests.Assessments
{
    public class When_Upload_File_IsValid : AssessmentsCsvHelperServiceBaseTest
    {
        private const string _dataFilePath = @"CommonServices\CsvHelperServiceTests\Assessments\TestData\Assessments_Stage2_Valid_File.csv";

        public override void Given()
        {
            Logger = new Logger<CsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse>>(new NullLoggerFactory());
            DataParser = new AssessmentParser();
            Validator = new AssessmentValidator();
            Service = new CsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse>(Validator, DataParser, Logger);
            FilePath = Path.Combine(Path.GetDirectoryName(GetCodeBaseAbsolutePath()), _dataFilePath);
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            await WhenAsync();
            ReadAndParseFileResponse.Should().NotBeNull();
            ReadAndParseFileResponse.IsDirty.Should().BeFalse();
            ReadAndParseFileResponse.ErrorMessage.Should().BeNullOrWhiteSpace();
            ReadAndParseFileResponse.Rows.Count.Should().Be(10);
            ReadAndParseFileResponse.Rows.Any(r => r.ValidationErrors.Count > 0).Should().BeFalse();
        }
    }
}
