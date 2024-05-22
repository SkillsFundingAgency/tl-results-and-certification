using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using Const = Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Constants;

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
                .MustBeNumberWithLength(Const.UlnLength);

            // Core code
            RuleFor(r => r.CoreCode)
                .Cascade(CascadeMode.Stop)
                .Required()
                .WithMessage(ValidationMessages.IpBulkCorecodeRequired)
                .MustBeStringWithLength(Const.CoreCodeLength)
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
                .Must(r =>
                {
                    IList<string> list = CsvStringToListParser.Parse(r);
                    return list.Any() && list.All(a => a.Length > 0);
                })
                .WithMessage(ValidationMessages.IpBulkReasonRequired)
                .When(x => x.IndustryPlacementStatus.Equals(EnumExtensions.GetDisplayName(IndustryPlacementStatus.CompletedWithSpecialConsideration), StringComparison.InvariantCultureIgnoreCase));

            RuleFor(r => r.SpecialConsiderationReasons)
                .MustBeValidSpecialConditionReason()
                .WithMessage(ValidationMessages.IpBulkReasonMustBeValid)
                .When(x => x.IndustryPlacementStatus.Equals(EnumExtensions.GetDisplayName(IndustryPlacementStatus.CompletedWithSpecialConsideration), StringComparison.InvariantCultureIgnoreCase));

            RuleFor(r => r.SpecialConsiderationReasons)
                .NotDuplicatesInCommaSeparatedString()
                .WithMessage(ValidationMessages.IpBulkReasonDuplicated)
                .When(r => !string.IsNullOrWhiteSpace(r.SpecialConsiderationReasons) && CsvStringToListParser.Parse(r.SpecialConsiderationReasons).Count > 1);
        }
    }
}