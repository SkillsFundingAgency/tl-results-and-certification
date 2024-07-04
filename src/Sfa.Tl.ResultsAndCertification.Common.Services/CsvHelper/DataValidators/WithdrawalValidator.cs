using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System.Linq;
using Const = Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Constants;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators
{
    public class WithdrawalValidator : AbstractValidator<WithdrawalCsvRecordRequest>
    {
        public WithdrawalValidator()
        {
            // Uln
            RuleFor(r => r.Uln)
                .Cascade(CascadeMode.Stop)
                .Required()
                .MustBeNumberWithLength(Const.UlnLength);

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
                .MustBeStringWithLength(Const.CoreCodeLength);

            // Specialisms
            RuleFor(r => r.Specialisms)
                .Must(x => CsvStringToListParser.Parse(x).All(a => a.Length == Const.SpecialismCodeLength))
                .WithMessage(string.Format(ValidationMessages.MustBeStringWithLength, "{PropertyName}", Const.SpecialismCodeLength))
                .When(r => !string.IsNullOrWhiteSpace(r.Specialisms));

            RuleFor(r => r.Specialisms)
                .NotDuplicatesInCommaSeparatedString()
                .WithMessage(ValidationMessages.SpecialismIsNotValid)
                .When(r => !string.IsNullOrWhiteSpace(r.Specialisms) && CsvStringToListParser.Parse(r.Specialisms).Count > 1);
        }
    }
}
