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

            // Todo: use extensions
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
                //RowNum = rownum,

                //ValidationErrors = new List<ValidationError>(reg.ValidationErrors)
                 ValidationErrors = new List<ValidationError>()
            };
        }

        public RegistrationCsvRecordResponse ParseErrorObject(int rownum, FileBaseModel model, ValidationResult validationResult)
        {
            if (!(model is RegistrationCsvRecordRequest reg))
                return null;

            return new RegistrationCsvRecordResponse
            {
                ValidationErrors = BuildValidationError(rownum, reg.Uln, validationResult)
            };
        }

        private IList<ValidationError> BuildValidationError(int rownum, string uln, ValidationResult validationResult)
        {
            var validationErrors = new List<ValidationError>();
            
            foreach (var err in validationResult.Errors)
            {
                validationErrors.Add(new ValidationError
                {
                    RowNum = rownum,
                    RowRef = uln,
                    ErrorMessage = err.ErrorMessage
                });
            }
            return validationErrors;
        }
    }
}
