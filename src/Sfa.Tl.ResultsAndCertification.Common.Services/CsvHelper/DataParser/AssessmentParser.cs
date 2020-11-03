using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser
{
    public class AssessmentParser : IDataParser<AssessmentCsvRecordResponse>
    {
        public AssessmentCsvRecordResponse ParseRow(FileBaseModel model, int rownum)
        {
            if (!(model is AssessmentCsvRecordRequest assessment))
                return null;

            return new AssessmentCsvRecordResponse
            {
                Uln = assessment.Uln.Trim().ToLong(),
                // TODO
                RowNum = rownum,
                ValidationErrors = new List<BulkProcessValidationError>()
            };
        }

        public AssessmentCsvRecordResponse ParseErrorObject(int rownum, FileBaseModel model, ValidationResult validationResult, string errorMessage = null)
        {
            var assessment = model as AssessmentCsvRecordRequest;
            var ulnValue = assessment != null && assessment.Uln.IsLong() ? assessment.Uln.ToLong() : 0;

            return new AssessmentCsvRecordResponse
            {
                // Note: Uln mapped here to use when checking Duplicate Uln and RowNum required at Stage-3 as well.
                Uln = ulnValue,
                RowNum = rownum,

                ValidationErrors = BuildValidationError(rownum, ulnValue, validationResult, errorMessage)
            };
        }

        private IList<BulkProcessValidationError> BuildValidationError(int rownum, long uln, ValidationResult validationResult, string errorMessage)
        {
            //TODO: this should be a common method for all parsers. 
            var validationErrors = new List<BulkProcessValidationError>();

            var errors = validationResult?.Errors?.Select(x => x.ErrorMessage) ?? new List<string> { errorMessage };

            foreach (var err in errors)
            {
                validationErrors.Add(new BulkProcessValidationError
                {
                    RowNum = rownum != 0 ? rownum.ToString() : string.Empty,
                    Uln = uln != 0 ? uln.ToString() : string.Empty,
                    ErrorMessage = err
                });
            }
            return validationErrors;
        }
    }
}
