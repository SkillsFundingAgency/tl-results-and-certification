using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser
{
    public class BaseParser
    {
        public IList<BulkProcessValidationError> BuildValidationError(int rownum, long uln, ValidationResult validationResult, string errorMessage)
        {
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
