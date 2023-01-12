using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
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
                .MustBeNumberWithLength(10);

            // Core code
            RuleFor(r => r.CoreCode)
                .Cascade(CascadeMode.Stop)
                .Required()
                .MustBeStringWithLength(8);

            // IndustryPlacementStatus
            RuleFor(r => r.IndustryPlacementStatus)
                .Cascade(CascadeMode.Stop)
                .Required();

            // IndustryPlacementHours
            RuleFor(r => r.IndustryPlacementHours)
                .MustBeNullOrEmpty()
                .When(x => !string.IsNullOrWhiteSpace(x.IndustryPlacementStatus) && !x.IndustryPlacementStatus.Equals("Placement completed with special considerations", StringComparison.InvariantCultureIgnoreCase));
                       
            RuleFor(r => r.IndustryPlacementHours)
                .MustBeNumberWithInRange(1, 999)
                .When(x => x.IndustryPlacementStatus.Equals("Placement completed with special considerations", StringComparison.InvariantCultureIgnoreCase));

            // SpecialConsiderationReasons
            RuleFor(r => r.SpecialConsiderationReasons)
               .MustBeNullOrEmpty()
               .When(x => !string.IsNullOrWhiteSpace(x.IndustryPlacementStatus) && !x.IndustryPlacementStatus.Equals("Placement completed with special considerations", StringComparison.InvariantCultureIgnoreCase));

            RuleFor(r => r.SpecialConsiderationReasons)                
                .Must(r => !string.IsNullOrWhiteSpace(r.Trim()) && r.Split(',').Where(s => !string.IsNullOrWhiteSpace(s.Trim())).All(a => a.Trim().Length > 0))
                .WithMessage(ValidationMessages.SpecialConsiderationReasonNeedsToBeProvided)
                .When(x => x.IndustryPlacementStatus.Equals("Placement completed with special considerations", StringComparison.InvariantCultureIgnoreCase));

            RuleFor(r => r.SpecialConsiderationReasons)
                .Must(r => !IsDuplicate(r))
                .WithMessage(ValidationMessages.SpecialConsiderationReasonIsNotValid)
                .When(r => !string.IsNullOrWhiteSpace(r.SpecialConsiderationReasons) && r.SpecialConsiderationReasons.Split(',').Count() > 1);            
        }

        private bool IsDuplicate(string commaSeparatedString)
        {
            return commaSeparatedString.Split(',').GroupBy(spl => spl.Trim()).Any(c => c.Count() > 1);
        }
    }
}
