using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators
{
    public class RegistrationValidator : AbstractValidator<RegistrationCsvRecordRequest>
    {
        public RegistrationValidator()
        {
            // Uln
            RuleFor(r => r.Uln)
                .Cascade(CascadeMode.Stop)
                .Required()
                .MustBeNumberWithLength(10);

            // Firstname
            RuleFor(r => r.FirstName)
                .Cascade(CascadeMode.Stop)
                .Required()
                .MaxStringLength(100);

            // Lastname
            RuleFor(r => r.LastName)
                .Cascade(CascadeMode.Stop)
                .Required()
                .MaxStringLength(100);

            // DateofBirth
            RuleFor(r => r.DateOfBirth)
                .Cascade(CascadeMode.Stop)
                .Required()
                .ValidDate()
                .NotFutureDate();

            // Ukprn
            RuleFor(r => r.Ukprn)
                .Cascade(CascadeMode.Stop)
                .Required()
                .MustBeNumberWithLength(8, ValidationMessages.MustBeAnNumberWithLength);

            // Academic year
            RuleFor(r => r.AcademicYear)
                .Cascade(CascadeMode.Stop)
                .Required()
                .MustBeInAcademicYearPattern();

            // Core
            RuleFor(r => r.Core)
                .Cascade(CascadeMode.Stop)
                .Required()
                .MustBeStringWithLength(8);

            // Specialisms
            RuleFor(r => r.Specialisms)
                .Must(x => x.Split(',').Where(s => !string.IsNullOrWhiteSpace(s.Trim())).All(a => a.Trim().Length == 8))
                .WithMessage(string.Format(ValidationMessages.MustBeStringWithLength, "{PropertyName}", 8))
                .When(r => !string.IsNullOrWhiteSpace(r.Specialisms));

            RuleFor(r => r.Specialisms)
                .Must(spl => !IsDuplicate(spl))
                .WithMessage(ValidationMessages.SpecialismIsNotValid)
                .When(r => !string.IsNullOrWhiteSpace(r.Specialisms) && r.Specialisms.Split(',').Count() > 1);
        }

        private bool IsDuplicate(string commaSeparatedString)
        {
            return commaSeparatedString.Split(',').GroupBy(spl => spl.Trim()).Any(c => c.Count() > 1);
        }
    }
}
