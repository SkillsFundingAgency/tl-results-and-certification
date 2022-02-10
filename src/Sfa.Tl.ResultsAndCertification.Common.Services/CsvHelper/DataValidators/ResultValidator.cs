using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators
{
    public class ResultValidator : AbstractValidator<ResultCsvRecordRequest>
    {
        public ResultValidator()
        {
            // Uln
            RuleFor(r => r.Uln)
                .Cascade(CascadeMode.Stop)
                .Required()
                .MustBeNumberWithLength(10);

            // Core code
            RuleFor(r => r.CoreCode)
                .MustBeStringWithLength(8)
                .When(x => !string.IsNullOrWhiteSpace(x.CoreCode));
            RuleFor(r => r.CoreCode)
                .Required()
                .WithMessage(ValidationMessages.CorecodeRequiredWhenResultIncluded)
                .When(x => !string.IsNullOrWhiteSpace(x.CoreAssessmentSeries) || !string.IsNullOrWhiteSpace(x.CoreGrade));
            RuleFor(r => r.CoreCode)
                .Required()
                .WithMessage(ValidationMessages.NoResultDataAfterUln)
                .When(x => !string.IsNullOrWhiteSpace(x.Uln) && string.IsNullOrWhiteSpace(x.CoreAssessmentSeries) && string.IsNullOrWhiteSpace(x.CoreGrade) 
                        && string.IsNullOrWhiteSpace(x.SpecialismCodes) && string.IsNullOrWhiteSpace(x.SpecialismSeries) && string.IsNullOrWhiteSpace(x.SpecialismGrades));

            // Core Assessment Series
            RuleFor(r => r.CoreAssessmentSeries)
                .Required()
                .WithMessage(ValidationMessages.AssessmentSeriesNeedsToBeProvided)
                .When(x => !string.IsNullOrWhiteSpace(x.CoreCode) || !string.IsNullOrWhiteSpace(x.CoreGrade));
            RuleFor(r => r.CoreAssessmentSeries)
                .MusBeValidAssessmentSeries()
                .WithMessage(ValidationMessages.InvalidCoreAssessmentSeries)
                .When(x => !string.IsNullOrWhiteSpace(x.CoreAssessmentSeries));

            // SpecialismCodes
            RuleFor(r => r.SpecialismCodes)
                .Must(x => x.Split(',').All(c => c.Trim().Length == 8))
                .WithMessage(string.Format(ValidationMessages.MustBeStringWithLength, "{PropertyName}", 8))
                .When(r => !string.IsNullOrWhiteSpace(r.SpecialismCodes));
            RuleFor(r => r.SpecialismCodes)
                .Required()
                .WithMessage(ValidationMessages.SpecialismCodeMustBeProvided) 
                .When(x => !string.IsNullOrWhiteSpace(x.SpecialismSeries) || !string.IsNullOrWhiteSpace(x.SpecialismGrades));
            RuleFor(r => r.SpecialismCodes)
                .Must(spl => !IsDuplicate(spl))
                .WithMessage(ValidationMessages.SpecialismCodesMustBeDifferent)
                .When(r => !string.IsNullOrWhiteSpace(r.SpecialismCodes) && r.SpecialismCodes.Split(',').Count() > 1);

            // SpecialismSeries
            RuleFor(r => r.SpecialismSeries)
                .Required()
                .WithMessage(ValidationMessages.SpecialismSeriesRequired)
                .When(x => !string.IsNullOrWhiteSpace(x.SpecialismCodes) || !string.IsNullOrWhiteSpace(x.SpecialismGrades));
            RuleFor(r => r.SpecialismSeries)
                .MusBeValidAssessmentSeries()
                .WithMessage(ValidationMessages.SpecialismSeriesInvalidFormat)
                .When(x => !string.IsNullOrWhiteSpace(x.SpecialismSeries));

            // SpecialismGrades
            RuleFor(r => r.SpecialismGrades)
                .Must((row, grades) => grades.Split(',').Count() == row.SpecialismCodes.Split(',').Count()) 
                .WithMessage(ValidationMessages.SpecialismGradeCountMismatch)
                .When(r => !string.IsNullOrWhiteSpace(r.SpecialismGrades));
        }

        private bool IsDuplicate(string commaSeparatedString)
        {
            return commaSeparatedString.Split(',').GroupBy(spl => spl.Trim()).Any(c => c.Count() > 1);
        }
    }
}
