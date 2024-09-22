﻿using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.PostResultsService.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser
{
    public class RommParser : BaseParser, IDataParser<RommCsvRecordResponse>
    {
        public RommCsvRecordResponse ParseRow(FileBaseModel model, int rownum)
        {
            if (model is not RommsCsvRecordRequest reg)
                return null;

            return new RommCsvRecordResponse
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

        public RommCsvRecordResponse ParseErrorObject(int rownum, FileBaseModel model, ValidationResult validationResult, string errorMessage = null)
        {
            var ulnValue = model is RommsCsvRecordRequest reg && reg.Uln.IsLong() ? reg.Uln.ToLong() : 0;

            return new RommCsvRecordResponse
            {
                // Note: Uln mapped here to use when checking Duplicate Uln and RowNum required at Stage-3 as well.
                Uln = ulnValue,
                RowNum = rownum,
                ValidationErrors = BuildValidationError(rownum, ulnValue, validationResult, errorMessage)
            };
        }
    }
}
