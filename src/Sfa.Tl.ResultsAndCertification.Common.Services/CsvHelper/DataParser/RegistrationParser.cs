using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser
{
    public class RegistrationParser : IDataParser<RegistrationCsvRecordResponse>
    {
        public RegistrationCsvRecordResponse ParseRow(FileBaseModel model, int rownum)
        {
            if (!(model is RegistrationCsvRecordRequest reg))
                return null;

            DateTime.TryParseExact(reg.DateOfBirth.Trim(), "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob);
            DateTime.TryParseExact(reg.StartDate.Trim(), "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);

            return new RegistrationCsvRecordResponse
            {
                Uln = int.Parse(reg.Uln.Trim()),
                FirstName = reg.FirstName.Trim(),
                LastName = reg.LastName.Trim(),
                DateOfBirth = dob,
                Ukprn = reg.Ukprn.Trim().ToLong(),
                StartDate = startDate,
                Core = reg.Core.Trim(),
                Specialisms = reg.Specialisms.Trim().Split(',').Where(s => !string.IsNullOrWhiteSpace(s.Trim())),

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
                Uln = ulnFound ? uln : 0,
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
