using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Model.Registration;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators
{
    public class RegistrationValidator : AbstractValidator<RegistrationCsvRecordRequest>
    {
        public RegistrationValidator()
        {
            // Uln
            RuleFor(r => r.Uln)
                .Required()
                .MustBeNumberWithLength(10);

            // Firstname
            RuleFor(r => r.FirstName)
                .Required()
                .MaxStringLength(256);

            // Lastname
            RuleFor(r => r.LastName)
                .Required()
                .MaxStringLength(256);

            // DateofBirth
            RuleFor(r => r.DateOfBirth)
                .Required()
                .ValidDate()
                .NotFutureDate();

            // Ukprn
            RuleFor(r => r.Ukprn)
                .Required()
                .MustBeNumberWithLength(8);

            // Startdate
            RuleFor(r => r.StartDate)
                .Required()
                .ValidDate();

            // Core
            RuleFor(r => r.Core)
                .Required()
                .MustBeStringWithLength(8);

            // Specialisms
            RuleFor(r => r.Specialisms)
                .Required()
                .Must(x => x.Split(',').Where(s => !string.IsNullOrWhiteSpace(s.Trim())).All(a =>  a.Trim().Length == 8))
                .WithMessage(string.Format(ValidationMessages.MustBeStringWithLength, "{PropertyName}", 8))
                .When(r => !string.IsNullOrWhiteSpace(r.Specialisms));
        }
    }
}
