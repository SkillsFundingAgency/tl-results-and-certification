using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataParser;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.PostResultsService.BulkProcess;
using System.Linq;
using Const = Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Constants;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.DataValidators
{
    public class RommValidator : AbstractValidator<RommsCsvRecordRequest>
    {
        public RommValidator()
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
            RuleFor(r => r.Specialism)
                .Must(x => CsvStringToListParser.Parse(x).All(a => a.Length == Const.SpecialismCodeLength))
                .WithMessage(string.Format(ValidationMessages.MustBeStringWithLength, "{PropertyName}", Const.SpecialismCodeLength))
                .When(r => !string.IsNullOrWhiteSpace(r.Specialism));

            RuleFor(r => r.Specialism)
                .NotDuplicatesInCommaSeparatedString()
                .WithMessage(ValidationMessages.SpecialismIsNotValid)
                .When(r => !string.IsNullOrWhiteSpace(r.Specialism) && CsvStringToListParser.Parse(r.Specialism).Count > 1);
        }
    }
}
