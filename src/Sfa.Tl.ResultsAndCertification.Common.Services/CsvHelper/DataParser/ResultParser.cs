using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser
{
    public class ResultParser : BaseParser, IDataParser<ResultCsvRecordResponse>
    {
        public ResultCsvRecordResponse ParseRow(FileBaseModel model, int rownum)
        {
            if (!(model is ResultCsvRecordRequest result))
                return null;

            return new ResultCsvRecordResponse
            {
                Uln = result.Uln.Trim().ToLong(),
                CoreAssessmentSeries = result.CoreAssessmentSeries.Trim(),
                RowNum = rownum,
                ValidationErrors = new List<BulkProcessValidationError>()
            };
        }

        public ResultCsvRecordResponse ParseErrorObject(int rownum, FileBaseModel model, ValidationResult validationResult, string errorMessage = null)
        {
            var result = model as ResultCsvRecordRequest;
            var ulnValue = result != null && result.Uln.IsLong() ? result.Uln.ToLong() : 0;

            return new ResultCsvRecordResponse
            {
                // Note: Uln mapped here to use when checking Duplicate Uln and RowNum required at Stage-3 as well.
                Uln = ulnValue,
                RowNum = rownum,

                ValidationErrors = BuildValidationError(rownum, ulnValue, validationResult, errorMessage)
            };
        }
    }
}
