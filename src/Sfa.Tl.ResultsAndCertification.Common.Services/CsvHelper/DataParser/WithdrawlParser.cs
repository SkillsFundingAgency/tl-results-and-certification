using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser
{
    public class WithdrawlParser : BaseParser, IDataParser<WithdrawlCsvRecordResponse>
    {
        public WithdrawlCsvRecordResponse ParseRow(FileBaseModel model, int rownum)
        {
            if (model is not WithdrawlCsvRecordRequest reg)
                return null;

            return new WithdrawlCsvRecordResponse
            {
                Uln = reg.Uln.Trim().ToLong(),
                FirstName = reg.FirstName.Trim(),
                LastName = reg.LastName.Trim(),
                DateOfBirth = reg.DateOfBirth.Trim().ParseStringToDateTime(),
                ProviderUkprn = reg.Ukprn.Trim().ToLong(),
                AcademicYearName = reg.AcademicYear.Trim(),
                CoreCode = reg.Core.Trim(),
                SpecialismCodes = CsvStringToListParser.Parse(reg.Specialisms),
                RowNum = rownum,
                ValidationErrors = new List<BulkProcessValidationError>()
            };
        }

        public WithdrawlCsvRecordResponse ParseErrorObject(int rownum, FileBaseModel model, ValidationResult validationResult, string errorMessage = null)
        {
            var ulnValue = model is WithdrawlCsvRecordRequest reg && reg.Uln.IsLong() ? reg.Uln.ToLong() : 0;

            return new WithdrawlCsvRecordResponse
            {
                // Note: Uln mapped here to use when checking Duplicate Uln and RowNum required at Stage-3 as well.
                Uln = ulnValue,
                RowNum = rownum,
                ValidationErrors = BuildValidationError(rownum, ulnValue, validationResult, errorMessage)
            };
        }
    }
}
