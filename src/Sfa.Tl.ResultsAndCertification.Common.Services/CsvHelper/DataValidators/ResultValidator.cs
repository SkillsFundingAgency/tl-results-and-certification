using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Result.BulkProcess;
using System.Linq;
using Const = Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Constants;

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
                .MustBeNumberWithLength(Const.UlnLength);

            // Core code
            RuleFor(r => r.CoreCode)
                .MustBeStringWithLength(Const.CoreCodeLength)
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
                .Must(x => CsvStringToListParser.Parse(x).All(c => c.Length == Const.SpecialismCodeLength))
                .WithMessage(string.Format(ValidationMessages.MustBeStringWithLength, "{PropertyName}", Const.SpecialismCodeLength))
                .When(r => !string.IsNullOrWhiteSpace(r.SpecialismCodes));

            RuleFor(r => r.SpecialismCodes)
                .Required()
                .WithMessage(ValidationMessages.SpecialismCodeMustBeProvided)
                .When(x => !string.IsNullOrWhiteSpace(x.SpecialismSeries) || !string.IsNullOrWhiteSpace(x.SpecialismGrades));

            RuleFor(r => r.SpecialismCodes)
                .NotDuplicatesInCommaSeparatedString()
                .WithMessage(ValidationMessages.SpecialismCodesMustBeDifferent)
                .When(r => !string.IsNullOrWhiteSpace(r.SpecialismCodes) && CsvStringToListParser.Parse(r.SpecialismCodes).Count > 1);

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
                .Must((row, grades) => CsvStringToListParser.Parse(grades).Count == CsvStringToListParser.Parse(row.SpecialismCodes).Count)
                .WithMessage(ValidationMessages.SpecialismGradeCountMismatch)
                .When(r => !string.IsNullOrWhiteSpace(r.SpecialismGrades));
        }
    }
}
