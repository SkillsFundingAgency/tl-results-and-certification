using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders.BulkRegistrations.ValidationErrorsBuilder;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices.CsvHelperServiceTests.Registrations
{
    public class When_Upload_File_Has_Errors : RegistrationsCsvHelperServiceBaseTest
    {
        private const string _dataFilePath = @"CommonServices\CsvHelperServiceTests\Registrations\TestData\Registrations_Stage_2_Validation.csv";
        private IList<RegistrationValidationError> _expectedValidationErrors;

        public override void Given()
        {
            Logger = new Logger<CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>>(new NullLoggerFactory());
            DataParser = new RegistrationParser();
            Validator = new RegistrationValidator();
            Service = new CsvHelperService<RegistrationCsvRecordRequest, CsvResponseModel<RegistrationCsvRecordResponse>, RegistrationCsvRecordResponse>(Validator, DataParser, Logger);
            FilePath = Path.Combine(Path.GetDirectoryName(GetCodeBaseAbsolutePath()), _dataFilePath);
            _expectedValidationErrors = new BulkRegistrationValidationErrorsBuilder().BuildValidationErrorsList();
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ReadAndParseFileResponse.Should().NotBeNull();
            ReadAndParseFileResponse.Rows.Count.Should().Be(1);

            var actualValidationErrors = ReadAndParseFileResponse.Rows.First().ValidationErrors;
            actualValidationErrors.Count.Should().Be(_expectedValidationErrors.Count);
            actualValidationErrors.Should().BeEquivalentTo(_expectedValidationErrors);
        }
    }
}
