using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Service;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices.CsvHelperServiceTests.Assessments
{
    public class When_Upload_File_Has_Errors : AssessmentsCsvHelperServiceBaseTest
    {
        private const string _dataFilePath = @"CommonServices\CsvHelperServiceTests\Assessments\TestData\Assessments_Stage_2_Validation.csv";
        private IList<BulkProcessValidationError> _expectedValidationErrors;        

        public override void Given()
        {
            Logger = new Logger<CsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse>>(new NullLoggerFactory());
            DataParser = new AssessmentParser();
            Validator = new AssessmentValidator();
            Service = new CsvHelperService<AssessmentCsvRecordRequest, CsvResponseModel<AssessmentCsvRecordResponse>, AssessmentCsvRecordResponse>(Validator, DataParser, Logger);
            FilePath = Path.Combine(Path.GetDirectoryName(GetCodeBaseAbsolutePath()), _dataFilePath);
            _expectedValidationErrors = GetExpectedValidationErrors();
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            await WhenAsync();
            ReadAndParseFileResponse.Should().NotBeNull();
            ReadAndParseFileResponse.Rows.Count.Should().Be(_expectedValidationErrors.Count);

            for (var i=0; i< _expectedValidationErrors.Count; i++)
            {
                var actualError = ReadAndParseFileResponse.Rows[i].ValidationErrors.First();
                
                actualError.RowNum.ToString().Should().Be(_expectedValidationErrors[i].RowNum);
                actualError.Uln.ToString().Should().Be(_expectedValidationErrors[i].Uln);
                actualError. ErrorMessage.Should().Be(_expectedValidationErrors[i].ErrorMessage);
            }
        }

        private IList<BulkProcessValidationError> GetExpectedValidationErrors()
        {
            var validationErrors = new List<BulkProcessValidationError>
            {
                new BulkProcessValidationError { RowNum = "2", Uln = string.Empty, ErrorMessage = "ULN required" },
                new BulkProcessValidationError { RowNum = "3", Uln = "123", ErrorMessage = "ULN must be a 10 digit number" },
                new BulkProcessValidationError { RowNum = "4", Uln = "1234567890", ErrorMessage = "Core code must have 8 digits only" },
                new BulkProcessValidationError { RowNum = "5", Uln = "1234567891", ErrorMessage = "Core code required when core assessment entry is included" },
                new BulkProcessValidationError { RowNum = "6", Uln = "1234567892", ErrorMessage = "Specialism code required when core assessment entry is included" },
                new BulkProcessValidationError { RowNum = "7", Uln = "1234567893", ErrorMessage = "Specialism code must have 8 characters only" },
                new BulkProcessValidationError { RowNum = "8", Uln = "1234567894", ErrorMessage = "Core assessment entry format must be text followed by a space and a 4-digit year" },
                new BulkProcessValidationError { RowNum = "9", Uln = "1234567895", ErrorMessage = "Specialism assessment entry format must be text followed by a space and a 4-digit year" },
                new BulkProcessValidationError { RowNum = "10", Uln = string.Empty, ErrorMessage = "Data in more than the required 5 columns" },
                new BulkProcessValidationError { RowNum = "11", Uln = "1234567898", ErrorMessage = "No data after ULN - need at least one core code or one specialism code" }
            };

            return validationErrors;
        }       
    }
}
