using FluentValidation.Results;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser.Interfaces;
using System.Collections.Generic;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser
{
    public class AssessmentParser : BaseParser, IDataParser<AssessmentCsvRecordResponse>
    {
        public AssessmentCsvRecordResponse ParseRow(FileBaseModel model, int rownum)
        {
            if (!(model is AssessmentCsvRecordRequest assessment))
                return null;

            return new AssessmentCsvRecordResponse
            {
                Uln = assessment.Uln.Trim().ToLong(),
                CoreCode = assessment.CoreCode.Trim(),
                SpecialismCode = assessment.SpecialismCodes.Trim(),
                CoreAssessmentEntry = assessment.CoreAssessmentEntry.Trim(),
                SpecialismAssessmentEntry = assessment.SpecialismAssessmentEntry.Trim(),
                RowNum = rownum,
                ValidationErrors = new List<BulkProcessValidationError>()
            };
        }

        public AssessmentCsvRecordResponse ParseErrorObject(int rownum, FileBaseModel model, ValidationResult validationResult, string errorMessage = null)
        {
            var assessment = model as AssessmentCsvRecordRequest;
            var ulnValue = assessment != null && assessment.Uln.IsLong() ? assessment.Uln.ToLong() : 0;

            return new AssessmentCsvRecordResponse
            {
                // Note: Uln mapped here to use when checking Duplicate Uln and RowNum required at Stage-3 as well.
                Uln = ulnValue,
                RowNum = rownum,

                ValidationErrors = BuildValidationError(rownum, ulnValue, validationResult, errorMessage)
            };
        }
    }
}
