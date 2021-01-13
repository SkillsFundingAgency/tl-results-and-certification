using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators
{
    public class ResultValidator : AbstractValidator<ResultCsvRecordRequest>
    {
        public ResultValidator()
        {
            // Uln
            RuleFor(r => r.Uln)
                .Required()
                .MustBeNumberWithLength(10);

            // Core code
            RuleFor(r => r.CoreCode)
                .MustBeNumberWithLength(8, ValidationMessages.MustHaveDigitsWithLength)
                .When(x => !string.IsNullOrEmpty(x.CoreCode));
            RuleFor(r => r.CoreCode)
                .Required()
                .WithMessage(ValidationMessages.CorecodeRequiredWhenResultIncluded)
                .When(x => !string.IsNullOrEmpty(x.CoreAssessmentSeries) || !string.IsNullOrEmpty(x.CoreGrade));
            RuleFor(r => r.CoreCode)
                .Required()
                .WithMessage(ValidationMessages.NoDataAfterUlnNeedCoreCode)
                .When(x => !string.IsNullOrEmpty(x.Uln) && string.IsNullOrEmpty(x.CoreAssessmentSeries) && string.IsNullOrEmpty(x.CoreGrade));

            // Core Assessment Series
            RuleFor(r => r.CoreAssessmentSeries)
                .Required()
                .WithMessage(ValidationMessages.AssessmentSeriesNeedsToBeProvided)
                .When(x => !string.IsNullOrEmpty(x.CoreCode) || !string.IsNullOrEmpty(x.CoreGrade));
            RuleFor(r => r.CoreAssessmentSeries)
                .MusBeValidAssessmentSeries()
                .WithMessage(ValidationMessages.InvalidCoreAssessmentSeries)
                .When(x => !string.IsNullOrEmpty(x.CoreAssessmentSeries));
        }
    }
}
