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
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices.CsvHelperServiceTests.Results
{
    public class When_Upload_File_Has_Errors : ResultsCsvHelperServiceBaseTest
    {
        private const string _dataFilePath = @"CommonServices\CsvHelperServiceTests\Results\TestData\Results_Stage_2_Validation.csv";
        private IList<BulkProcessValidationError> _expectedValidationErrors;        

        public override void Given()
        {
            Logger = new Logger<CsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse>>(new NullLoggerFactory());
            DataParser = new ResultParser();
            Validator = new ResultValidator();
            Service = new CsvHelperService<ResultCsvRecordRequest, CsvResponseModel<ResultCsvRecordResponse>, ResultCsvRecordResponse>(Validator, DataParser, Logger);
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
                new BulkProcessValidationError { RowNum = "4", Uln = "1234567890", ErrorMessage = "Core component code must have 8 digits only" },
                new BulkProcessValidationError { RowNum = "5", Uln = "1234567891", ErrorMessage = "Core component code required when result is included" },
                new BulkProcessValidationError { RowNum = "6", Uln = "1234567892", ErrorMessage = "Assessment series needs to be provided" },
                new BulkProcessValidationError { RowNum = "7", Uln = "1234567893", ErrorMessage = "Core assessment series format must be text followed by a space and a 4-digit year" },
                new BulkProcessValidationError { RowNum = "8", Uln = "1234567894", ErrorMessage = "No data provided for this learner. Please provide data or remove the row." },
                new BulkProcessValidationError { RowNum = "9", Uln = string.Empty, ErrorMessage = "Data in more than the required 7 columns" },
                new BulkProcessValidationError { RowNum = "10", Uln = string.Empty, ErrorMessage = "ULN required" },

                // 6 more message to be added. 
                // File level validations
                // one validation at bulkloader
            };

            return validationErrors;
        }       
    }
}
