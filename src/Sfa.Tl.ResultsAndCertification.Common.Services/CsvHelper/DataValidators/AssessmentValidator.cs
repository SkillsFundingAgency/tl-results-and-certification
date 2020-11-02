using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators
{
    public class AssessmentValidator : AbstractValidator<AssessmentCsvRecordRequest>
    {
        private static readonly string assessmentEntryFormat = "^(summer|autumn) [0-9]{4}$";

        public AssessmentValidator()
        {
            // Uln
            RuleFor(r => r.Uln)
                .Required()
                .MustBeNumberWithLength(10);
            
            // CoreCode
            RuleFor(r => r.CoreCode)
                .MustBeNumberWithLength(8)
                .When(x => !string.IsNullOrEmpty(x.CoreCode));
            RuleFor(r => r.CoreCode)
                .Required()
                .WithMessage(ValidationMessages.CorecodeRequired)
                .When(x => !string.IsNullOrEmpty(x.CoreAssessmentEntry));
            RuleFor(r => r.CoreCode)
                .Must(x => !string.IsNullOrEmpty(x))
                .WithMessage(ValidationMessages.AtleastOneEntryRequired)
                .When(x => string.IsNullOrEmpty(x.SpecialismCode));

            // CoreAssessmentEntry
            RuleFor(r => r.CoreAssessmentEntry)
                .MustBeInPattern(assessmentEntryFormat)
                .WithMessage(ValidationMessages.CoreAssementEntryInvalidFormat)
                .When(x => !string.IsNullOrEmpty(x.CoreAssessmentEntry));

            // SpecialismCode
            RuleFor(r => r.SpecialismCode)
                .MustBeStringWithLength(8)
                .When(x => !string.IsNullOrEmpty(x.SpecialismCode));
            RuleFor(r => r.SpecialismCode)
                .Required()
                .WithMessage(ValidationMessages.SpecialismcodeRequired)
                .When(x => !string.IsNullOrEmpty(x.SpecialismAssessmentEntry));

            // SpecialismAssessmentEntry
            RuleFor(r => r.SpecialismAssessmentEntry)
                .MustBeInPattern(assessmentEntryFormat)
                .WithMessage(ValidationMessages.SpecialismAssementEntryInvalidFormat)
                .When(x => !string.IsNullOrEmpty(x.SpecialismAssessmentEntry));
        }
    }
}
