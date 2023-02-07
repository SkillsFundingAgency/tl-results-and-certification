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
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.IntegrationTests.CommonServices.CsvHelperServiceTests.IndustryPlacement
{
    public class When_Upload_File_Has_Errors : IndustryPlacementStatusCsvHelperServiceBaseTest
    {
        private const string _dataFilePath = @"CommonServices\CsvHelperServiceTests\IndustryPlacement\TestData\IndustryPlacement_Stage2_Errors.csv";
        private IList<BulkProcessValidationError> _expectedValidationErrors;        

        public override void Given()
        {
            Logger = new Logger<CsvHelperService<IndustryPlacementCsvRecordRequest, CsvResponseModel<IndustryPlacementCsvRecordResponse>, IndustryPlacementCsvRecordResponse>>(new NullLoggerFactory());
            DataParser = new IndustryPlacementParser();
            Validator = new IndustryPlacementValidator();
            Service = new CsvHelperService<IndustryPlacementCsvRecordRequest, CsvResponseModel<IndustryPlacementCsvRecordResponse>, IndustryPlacementCsvRecordResponse>(Validator, DataParser, Logger);
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
                // ULN
                new BulkProcessValidationError { RowNum = "2", Uln = "245234345633", ErrorMessage = "ULN must be a 10 digit number" },
                new BulkProcessValidationError { RowNum = "3", Uln = "24523434", ErrorMessage = "ULN must be a 10 digit number" },
                new BulkProcessValidationError { RowNum = "4", Uln = string.Empty, ErrorMessage = "Enter ULN" },
                new BulkProcessValidationError { RowNum = "5", Uln = string.Empty, ErrorMessage = "ULN must be a 10 digit number" },

                // Corecode
                new BulkProcessValidationError { RowNum = "6", Uln = "1234567890", ErrorMessage = "Enter core code" },
                new BulkProcessValidationError { RowNum = "7", Uln = "1234567891", ErrorMessage = "Core code must be 8 characters" },
                new BulkProcessValidationError { RowNum = "8", Uln = "1234567892", ErrorMessage = "Core code must be 8 characters" },
                new BulkProcessValidationError { RowNum = "9", Uln = "1234567893", ErrorMessage = "Core code must be 8 characters" },

                // Industryplacement Status
                new BulkProcessValidationError { RowNum = "10", Uln = "1111111111", ErrorMessage = "Industry placement status required" },
                new BulkProcessValidationError { RowNum = "11", Uln = "1111111112", ErrorMessage = "Industry placement status not recognised" },

                // Hours
                new BulkProcessValidationError { RowNum = "12", Uln = "2222222220", ErrorMessage = "Industry placement hours must be provided if industry placement status is Completed with special consideration" },
                new BulkProcessValidationError { RowNum = "13", Uln = "2222222221", ErrorMessage = "Industry placement hours must be empty unless industry placement status is Completed with special consideration" },
                new BulkProcessValidationError { RowNum = "14", Uln = "2222222222", ErrorMessage = "The placement duration must be a whole number between 1 and 999 hours" },

                // Reasons
                new BulkProcessValidationError { RowNum = "15", Uln = "3333333330", ErrorMessage = "Industry placement reasons must be empty unless industry placement status is Completed with special consideration" },
                new BulkProcessValidationError { RowNum = "16", Uln = "3333333331", ErrorMessage = "At least one industry placement reason must be provided if industry placement status is Completed with special consideration" },
                new BulkProcessValidationError { RowNum = "17", Uln = "3333333332", ErrorMessage = "Each industry placement reason can only be included once per learner" },
                new BulkProcessValidationError { RowNum = "18", Uln = "3333333333", ErrorMessage = "Invalid industry placement reason code" }
            };

            return validationErrors;
        }       
    }
}
