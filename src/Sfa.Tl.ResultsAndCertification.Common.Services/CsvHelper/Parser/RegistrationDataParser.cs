using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Parser
{
    public class RegistrationDataParser : IDataParser<Registration>
    {
        public Registration ParseRow(FileBaseModel model, int rownum)
        {
            if (!(model is RegistrationCsvRecord reg))
                return null;

            if (!reg.IsValid)
                return ParseErrors(reg.ValidationErrors);

            DateTime.TryParseExact(reg.DateOfBirth.Trim(), "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob);
            DateTime.TryParseExact(reg.StartDate.Trim(), "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);

            return new Registration
            {
                Uln = int.Parse(reg.Uln.Trim()),
                FirstName = reg.FirstName.Trim(),
                LastName = reg.LastName.Trim(),
                DateOfBirth = dob,
                Ukprn = reg.Ukprn.Trim().ToLong(),
                StartDate = startDate,
                Core = reg.Core.Trim(),
                Specialisms = reg.Specialisms.Trim().Split(',').Where(s => !string.IsNullOrWhiteSpace(s.Trim())),
                RowNum = rownum,

                ValidationErrors = new List<ValidationError>(reg.ValidationErrors)
            };
        }

        public Registration ParseErrors(IList<ValidationError> validationErrors)
        {
            return new Registration { ValidationErrors = validationErrors.ToList() };
        }

        public Registration ParseErrorObject<TImportModel>(int rownum, FileBaseModel model, ValidationResult validationResult)
        {
            return new Registration
            {
                RowNum = rownum,
                ValidationErrors = ParseValidationErrors(rownum, "", validationResult)
            };
        }

        private IList<ValidationError> ParseValidationErrors(int rownum, string uln, ValidationResult validationResult = null)
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
