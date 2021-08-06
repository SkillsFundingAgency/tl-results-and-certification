using FluentAssertions;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.IO;
using System.Linq;
using Xunit;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices.CsvHelperServiceTests.Registrations
{
    public class When_Upload_File_IsValid : RegistrationsCsvHelperServiceBaseTest
    {
        private const string _dataFilePath = @"CommonServices\CsvHelperServiceTests\Registrations\TestData\Registrations_Stage_2_Valid_File.csv";

        public override void Given()
        {
            Logger = new Logger<CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>>(new NullLoggerFactory());
            DataParser = new RegistrationParser();
            Validator = new RegistrationValidator();
            Service = new CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>(Validator, DataParser, Logger);
            FilePath = Path.Combine(Path.GetDirectoryName(GetCodeBaseAbsolutePath()), _dataFilePath);
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            await WhenAsync();
            ReadAndParseFileResponse.Should().NotBeNull();
            ReadAndParseFileResponse.IsDirty.Should().BeFalse();
            ReadAndParseFileResponse.ErrorMessage.Should().BeNullOrWhiteSpace();
            ReadAndParseFileResponse.Rows.Count.Should().Be(2);
            ReadAndParseFileResponse.Rows.Any(r => r.ValidationErrors.Count > 0).Should().BeFalse();
        }
    }
}
