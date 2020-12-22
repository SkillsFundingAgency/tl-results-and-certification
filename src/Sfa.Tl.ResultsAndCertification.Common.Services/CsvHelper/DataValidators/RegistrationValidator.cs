using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;

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
                .MaxStringLength(100);

            // Lastname
            RuleFor(r => r.LastName)
                .Required()
                .MaxStringLength(100);

            // DateofBirth
            RuleFor(r => r.DateOfBirth)
                .Required()
                .ValidDate()
                .NotFutureDate();

            // Ukprn
            RuleFor(r => r.Ukprn)
                .Required()
                .MustBeNumberWithLength(8, ValidationMessages.MustBeAnNumberWithLength);

            // Academic year
            RuleFor(r => r.AcademicYear)
                .Required()
                .MustBeInAcademicYearPattern()
                .MusBeValidAcademicYear();

            // Core
            RuleFor(r => r.Core)
                .Required()
                .MustBeStringWithLength(8);

            // Specialism
            RuleFor(r => r.Specialism)
                .MustBeStringWithLength(8)
                .When(x => !string.IsNullOrEmpty(x.Specialism));
        }
    }
}
