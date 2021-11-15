using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators
{
    public class AssessmentValidator : AbstractValidator<AssessmentCsvRecordRequest>
    {
        public AssessmentValidator()
        {
            // Uln
            RuleFor(r => r.Uln)
                .Cascade(CascadeMode.Stop)
                .Required()
                .MustBeNumberWithLength(10);

            // CoreCode
            RuleFor(r => r.CoreCode)
                .MustBeNumberWithLength(8)
                .WithMessage(ValidationMessages.CorecodeMustBeDigitsOnly)
                .When(x => !string.IsNullOrWhiteSpace(x.CoreCode));

            RuleFor(r => r.CoreCode)
                .Required()
                .WithMessage(ValidationMessages.CorecodeRequired)
                .When(x => !string.IsNullOrWhiteSpace(x.CoreAssessmentEntry));
            
            RuleFor(r => r.CoreCode)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage(ValidationMessages.NoDataAfterUln)
                .When(x => string.IsNullOrWhiteSpace(x.CoreAssessmentEntry) && string.IsNullOrWhiteSpace(x.SpecialismCodes) && string.IsNullOrWhiteSpace(x.SpecialismAssessmentEntry));

            // CoreAssessmentEntry
            RuleFor(r => r.CoreAssessmentEntry)
                .MusBeValidAssessmentSeries()
                .WithMessage(ValidationMessages.CoreAssementEntryInvalidFormat)
                .When(x => !string.IsNullOrWhiteSpace(x.CoreAssessmentEntry));

            // SpecialismCode
            RuleFor(r => r.SpecialismCodes)
                .Must(x => x.Split(',').Where(s => !string.IsNullOrWhiteSpace(s.Trim())).All(a => a.Trim().Length == 8))
                .WithMessage(string.Format(ValidationMessages.MustBeStringWithLength, "{PropertyName}", 8))
                .When(r => !string.IsNullOrWhiteSpace(r.SpecialismCodes));
            RuleFor(r => r.SpecialismCodes)
                .Required()
                .WithMessage(ValidationMessages.SpecialismcodeRequired)
                .When(x => !string.IsNullOrWhiteSpace(x.SpecialismAssessmentEntry));

            RuleFor(r => r.SpecialismCodes)
                .Must(spl => !IsDuplicate(spl))
                .WithMessage(ValidationMessages.SpecialismCodesMustBeDifferent)
                .When(r => !string.IsNullOrWhiteSpace(r.SpecialismCodes) && r.SpecialismCodes.Split(',').Count() > 1);

            // SpecialismAssessmentEntry
            RuleFor(r => r.SpecialismAssessmentEntry)
                .MusBeValidAssessmentSeries()
                .WithMessage(ValidationMessages.SpecialismAssementEntryInvalidFormat)
                .When(x => !string.IsNullOrWhiteSpace(x.SpecialismAssessmentEntry));
        }

        private bool IsDuplicate(string commaSeparatedString)
        {
            return commaSeparatedString.Split(',').GroupBy(spl => spl.Trim()).Any(c => c.Count() > 1);
        }
    }
}
