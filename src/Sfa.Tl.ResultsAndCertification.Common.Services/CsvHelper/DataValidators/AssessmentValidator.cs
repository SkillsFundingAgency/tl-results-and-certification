using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Assessment.BulkProcess;
using System.Linq;
using Const = Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Constants;

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
                .MustBeNumberWithLength(Const.UlnLength);

            // CoreCode
            RuleFor(r => r.CoreCode)
                .MustBeStringWithLength(Const.CoreCodeLength)
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
                .Required()
                .WithMessage(ValidationMessages.SpecialismcodeRequired)
                .When(x => !string.IsNullOrWhiteSpace(x.SpecialismAssessmentEntry));

            RuleFor(r => r.SpecialismCodes)
                .Must(x => CsvStringToListParser.Parse(x).All(a => a.Length == Const.SpecialismCodeLength))
                .WithMessage(string.Format(ValidationMessages.MustBeStringWithLength, "{PropertyName}", Const.SpecialismCodeLength))
                 .When(r => !string.IsNullOrWhiteSpace(r.SpecialismCodes));

            RuleFor(r => r.SpecialismCodes)
                .NotDuplicatesInCommaSeparatedString()
                .WithMessage(ValidationMessages.SpecialismCodesMustBeDifferent)
                .When(r => !string.IsNullOrWhiteSpace(r.SpecialismCodes) && CsvStringToListParser.Parse(r.SpecialismCodes).Count > 1);

            // SpecialismAssessmentEntry
            RuleFor(r => r.SpecialismAssessmentEntry)
                .MusBeValidAssessmentSeries()
                .WithMessage(ValidationMessages.SpecialismAssementEntryInvalidFormat)
                .When(x => !string.IsNullOrWhiteSpace(x.SpecialismAssessmentEntry));
        }
    }
}