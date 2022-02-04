using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Linq;
using Xunit;
using System.IO;
using System.Threading.Tasks;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices.CsvHelperServiceTests.Results
{
    public class When_Upload_File_IsValid : ResultsCsvHelperServiceBaseTest
    {
        private const string _dataFilePath = @"CommonServices\CsvHelperServiceTests\Results\TestData\Results_Stage2_Valid_File.csv";

        public override void Given()
        {
            Logger = new Logger<CsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse>>(new NullLoggerFactory());
            DataParser = new ResultParser();
            Validator = new ResultValidator();
            Service = new CsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse>(Validator, DataParser, Logger);
            FilePath = Path.Combine(Path.GetDirectoryName(GetCodeBaseAbsolutePath()), _dataFilePath);
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            await WhenAsync();
            ReadAndParseFileResponse.Should().NotBeNull();
            ReadAndParseFileResponse.IsDirty.Should().BeFalse();
            ReadAndParseFileResponse.ErrorMessage.Should().BeNullOrWhiteSpace();
            ReadAndParseFileResponse.Rows.Count.Should().Be(7);
            ReadAndParseFileResponse.Rows.Any(r => r.ValidationErrors.Count > 0).Should().BeFalse();
        }
    }
}
