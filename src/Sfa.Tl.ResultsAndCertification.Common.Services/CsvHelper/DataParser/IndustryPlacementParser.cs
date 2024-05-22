using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser
{
    public class IndustryPlacementParser : BaseParser, IDataParser<IndustryPlacementCsvRecordResponse>
    {
        public IndustryPlacementCsvRecordResponse ParseRow(FileBaseModel model, int rownum)
        {
            if (model is not IndustryPlacementCsvRecordRequest result)
                return null;

            return new IndustryPlacementCsvRecordResponse
            {
                Uln = result.Uln.Trim().ToLong(),
                CoreCode = result.CoreCode?.Trim(),
                IndustryPlacementStatus = result.IndustryPlacementStatus?.Trim(),
                IndustryPlacementHours = result.IndustryPlacementHours?.Trim(),
                SpecialConsiderations = CsvStringToListParser.Parse(result.SpecialConsiderationReasons),
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
