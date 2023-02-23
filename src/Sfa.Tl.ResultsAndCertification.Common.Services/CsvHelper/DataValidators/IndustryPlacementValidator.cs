using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;
using System;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators
{
    public class IndustryPlacementValidator : AbstractValidator<IndustryPlacementCsvRecordRequest>
    {
        public IndustryPlacementValidator()
        {
            // Uln
            RuleFor(r => r.Uln)
                .Cascade(CascadeMode.Stop)
                .Required()
                .WithMessage(ValidationMessages.IpBulkUlnRequired)
                .MustBeNumberWithLength(10);

            // Core code
            RuleFor(r => r.CoreCode)
                .Cascade(CascadeMode.Stop)
                .Required()
                .WithMessage(ValidationMessages.IpBulkCorecodeRequired)
                .MustBeStringWithLength(8)
                .WithMessage(ValidationMessages.IpBulkCorecodeMustBe8Chars);

            // IndustryPlacementStatus
            RuleFor(r => r.IndustryPlacementStatus)
                .Cascade(CascadeMode.Stop)
                .Required()
                .MustBeValidIndustryPlacementStatus()
                .WithMessage(ValidationMessages.IpBulkStatusMustBeValid);

            // IndustryPlacementHours
            RuleFor(r => r.IndustryPlacementHours)
                .Required()
                .WithMessage(ValidationMessages.IpBulkHoursRequired)
                .When(x => !string.IsNullOrWhiteSpace(x.IndustryPlacementStatus) &&
                           x.IndustryPlacementStatus.Equals(EnumExtensions.GetDisplayName(IndustryPlacementStatus.CompletedWithSpecialConsideration), StringComparison.InvariantCultureIgnoreCase) &&
                           string.IsNullOrWhiteSpace(x.IndustryPlacementHours));

            RuleFor(r => r.IndustryPlacementHours)
                .MustBeNullOrEmpty()
                .WithMessage(ValidationMessages.IpBulkHoursMustBeEmpty)
                .When(x => !string.IsNullOrWhiteSpace(x.IndustryPlacementStatus) &&
                           !x.IndustryPlacementStatus.Equals(EnumExtensions.GetDisplayName(IndustryPlacementStatus.CompletedWithSpecialConsideration), StringComparison.InvariantCultureIgnoreCase));

            RuleFor(r => r.IndustryPlacementHours)
                .MustBeNumberWithInRange(1, 999)
                .WithMessage(ValidationMessages.IpBulkHoursMustOutOfRange)
                .When(x => !string.IsNullOrWhiteSpace(x.IndustryPlacementStatus) && !string.IsNullOrWhiteSpace(x.IndustryPlacementHours) &&
                            x.IndustryPlacementStatus.Equals(EnumExtensions.GetDisplayName(IndustryPlacementStatus.CompletedWithSpecialConsideration), StringComparison.InvariantCultureIgnoreCase));

            // SpecialConsiderationReasons
            RuleFor(r => r.SpecialConsiderationReasons)
               .MustBeNullOrEmpty()
               .WithMessage(ValidationMessages.IpBulkReasonMustBeEmpty)
               .When(x => !string.IsNullOrWhiteSpace(x.IndustryPlacementStatus) &&
                           !x.IndustryPlacementStatus.Equals(EnumExtensions.GetDisplayName(IndustryPlacementStatus.CompletedWithSpecialConsideration), StringComparison.InvariantCultureIgnoreCase));

            RuleFor(r => r.SpecialConsiderationReasons)
                .Must(r => !string.IsNullOrWhiteSpace(r.Trim()) && r.Split(',').Where(s => !string.IsNullOrWhiteSpace(s.Trim())).All(a => a.Trim().Length > 0))
                .WithMessage(ValidationMessages.IpBulkReasonRequired)
                .When(x => x.IndustryPlacementStatus.Equals(EnumExtensions.GetDisplayName(IndustryPlacementStatus.CompletedWithSpecialConsideration), StringComparison.InvariantCultureIgnoreCase));

            RuleFor(r => r.SpecialConsiderationReasons)
                .MustBeValidSpecialConditionReason()
                .WithMessage(ValidationMessages.IpBulkReasonMustBeValid)
                .When(x => x.IndustryPlacementStatus.Equals(EnumExtensions.GetDisplayName(IndustryPlacementStatus.CompletedWithSpecialConsideration), StringComparison.InvariantCultureIgnoreCase));

            RuleFor(r => r.SpecialConsiderationReasons)
                .Must(r => !IsDuplicate(r))
                .WithMessage(ValidationMessages.IpBulkReasonDuplicated)
                .When(r => !string.IsNullOrWhiteSpace(r.SpecialConsiderationReasons) && r.SpecialConsiderationReasons.Split(',').Count() > 1);
        }

        private bool IsDuplicate(string commaSeparatedString)
        {
            return commaSeparatedString.Split(',').GroupBy(spl => spl.Trim()).Any(c => c.Count() > 1);
        }
    }
}
