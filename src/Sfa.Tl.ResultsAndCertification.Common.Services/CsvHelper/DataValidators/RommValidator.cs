﻿using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.PostResultsService.BulkProcess;
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

            // Core Assessment Series
            RuleFor(r => r.AssessmentSeriesCore)
                .Cascade(CascadeMode.Stop)
                .Required();

            // Core Component Code
            RuleFor(r => r.Core)
                .Cascade(CascadeMode.Stop)
                .Required()
                .MustBeStringWithLength(Const.CoreCodeLength);

            // Core Component Open
            RuleFor(r => r.CoreRommOpen)
                .Cascade(CascadeMode.Stop)
                .Required()
                .MustBeYesOrNoValidation();

            // Specialisms
            RuleFor(r => r.Specialism)
                .MustBeStringWithLength(Const.SpecialismCodeLength)
                .WithMessage(string.Format(ValidationMessages.MustBeStringWithLength, "{PropertyName}", Const.SpecialismCodeLength))
                .When(r => !string.IsNullOrWhiteSpace(r.Specialism));

            // Specialism Assessment Series
            RuleFor(r => r.AssessmentSeriesSpecialism)
                .NotNull()
                .When(r => !string.IsNullOrWhiteSpace(r.Specialism));

            // Specialism Component Open
            RuleFor(r => r.SpecialismRommOpen)
                .Cascade(CascadeMode.Stop)
                .Required()
                .MustBeYesOrNoValidation();


        }
    }
}
