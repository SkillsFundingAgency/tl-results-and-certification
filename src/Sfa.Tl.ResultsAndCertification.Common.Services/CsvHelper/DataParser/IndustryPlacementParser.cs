using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.Linq;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser
{
    public class IndustryPlacementParser : BaseParser, IDataParser<IndustryPlacementCsvRecordResponse>
    {
        public IndustryPlacementCsvRecordResponse ParseRow(FileBaseModel model, int rownum)
        {
            if (!(model is IndustryPlacementCsvRecordRequest result))
                return null;

            return new IndustryPlacementCsvRecordResponse
            {
                Uln = result.Uln.Trim().ToLong(),
                CoreCode = result.CoreCode?.Trim(),
                IndustryPlacementStatus = result.IndustryPlacementStatus?.Trim(),
                IndustryPlacementHours = result.IndustryPlacementHours?.Trim(),
                SpecialConsiderations = result.SpecialConsiderationReasons?.Trim()?.Split(',')?.Where(s => !string.IsNullOrWhiteSpace(s.Trim()))?.Select(sp => sp.Trim())?.ToList(),
                RowNum = rownum,
                ValidationErrors = new List<BulkProcessValidationError>()
            };
        }

        public IndustryPlacementCsvRecordResponse ParseErrorObject(int rownum, FileBaseModel model, ValidationResult validationResult, string errorMessage = null)
        {
            var result = model as IndustryPlacementCsvRecordRequest;
            var ulnValue = result != null && result.Uln.IsLong() ? result.Uln.ToLong() : 0;

            return new IndustryPlacementCsvRecordResponse
            {
                // Note: Uln mapped here to use when checking Duplicate Uln and RowNum required at Stage-3 as well.
                Uln = ulnValue,
                RowNum = rownum,

                ValidationErrors = BuildValidationError(rownum, ulnValue, validationResult, errorMessage)
            };
        }
    }
}
