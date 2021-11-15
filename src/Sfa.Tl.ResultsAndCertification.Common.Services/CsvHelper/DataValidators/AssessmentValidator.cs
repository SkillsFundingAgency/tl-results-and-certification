using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;

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
                .When(x => string.IsNullOrWhiteSpace(x.CoreAssessmentEntry) && string.IsNullOrWhiteSpace(x.SpecialismCode) && string.IsNullOrWhiteSpace(x.SpecialismAssessmentEntry));

            // CoreAssessmentEntry
            RuleFor(r => r.CoreAssessmentEntry)
                .MusBeValidAssessmentSeries()
                .WithMessage(ValidationMessages.CoreAssementEntryInvalidFormat)
                .When(x => !string.IsNullOrWhiteSpace(x.CoreAssessmentEntry));

            // SpecialismCode
            RuleFor(r => r.SpecialismCode)
                .MustBeStringWithLength(8)
                .When(x => !string.IsNullOrWhiteSpace(x.SpecialismCode));
            RuleFor(r => r.SpecialismCode)
                .Required()
                .WithMessage(ValidationMessages.SpecialismcodeRequired)
                .When(x => !string.IsNullOrWhiteSpace(x.SpecialismAssessmentEntry));

            // SpecialismAssessmentEntry
            RuleFor(r => r.SpecialismAssessmentEntry)
                .MusBeValidAssessmentSeries()
                .WithMessage(ValidationMessages.SpecialismAssementEntryInvalidFormat)
                .When(x => !string.IsNullOrWhiteSpace(x.SpecialismAssessmentEntry));
        }
    }
}
