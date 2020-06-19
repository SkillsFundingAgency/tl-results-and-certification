using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser
{
    public class RegistrationParser : IDataParser<RegistrationCsvRecordResponse>
    {
        public RegistrationCsvRecordResponse ParseRow(FileBaseModel model, int rownum)
        {
            if (!(model is RegistrationCsvRecordRequest reg))
                return null;

            return new RegistrationCsvRecordResponse
            {
                Uln = reg.Uln.Trim().ToInt(),
                FirstName = reg.FirstName.Trim(),
                LastName = reg.LastName.Trim(),
                DateOfBirth = reg.DateOfBirth.Trim().ParseStringToDateTime(),
                Ukprn = reg.Ukprn.Trim().ToLong(),
                StartDate = reg.StartDate.Trim().ParseStringToDateTime(),
                Core = reg.Core.Trim(),
                Specialisms = reg.Specialisms.Trim().Split(',').Where(s => !string.IsNullOrWhiteSpace(s.Trim())),
                RowNum = rownum,
                ValidationErrors = new List<RegistrationValidationError>()
            };
        }

        public RegistrationCsvRecordResponse ParseErrorObject(int rownum, FileBaseModel model, ValidationResult validationResult)
        {
            if (!(model is RegistrationCsvRecordRequest reg))
                return null;

            var ulnFound = int.TryParse(reg.Uln.Trim(), out int uln);
            return new RegistrationCsvRecordResponse
            {
                // Note: Uln mapped here to use when checking Duplicate Uln and RowNum required at Stage-3 as well.
                Uln = ulnFound ? uln : 0,
                RowNum = rownum,

                ValidationErrors = BuildValidationError(rownum, reg.Uln, validationResult)
            };
        }

        private IList<RegistrationValidationError> BuildValidationError(int rownum, string uln, ValidationResult validationResult)
        {
            var validationErrors = new List<RegistrationValidationError>();
            
            foreach (var err in validationResult.Errors)
            {
                validationErrors.Add(new RegistrationValidationError
                {
                    RowNum = rownum != 0 ? rownum.ToString() : string.Empty,
                    Uln = uln,
                    ErrorMessage = err.ErrorMessage
                });
            }
            return validationErrors;
        }
    }
}
