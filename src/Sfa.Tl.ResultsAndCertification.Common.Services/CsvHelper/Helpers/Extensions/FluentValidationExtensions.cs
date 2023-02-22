using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Constants;
using System;
using System.Text.RegularExpressions;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions
{
    public static class FluentValidationExtensions
    {
        private static readonly string academicYearPattern = "^[0-9]{4}/[0-9]{2}/?$";
        private static readonly string assessmentEntryFormat = "^[a-z]{1,20} [0-9]{4}$";

        public static IRuleBuilderOptions<T, string> Required<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .WithMessage(string.Format(ValidationMessages.Required, "{PropertyName}"));
        }

        public static IRuleBuilderOptions<T, string> MustBeNumberWithLength<T>(this IRuleBuilder<T, string> ruleBuilder, int length, string message = null)
        {
            return ruleBuilder
                .Must(r => r.Length == length && r.IsLong())
                .WithMessage(string.Format(!string.IsNullOrWhiteSpace(message) ? message : ValidationMessages.MustBeNumberWithLength, "{PropertyName}", length));
        }

        public static IRuleBuilderOptions<T, string> MustBeStringWithLength<T>(this IRuleBuilder<T, string> ruleBuilder, int length)
        {
            return ruleBuilder
                .Must(r => r.Length == length)
                .WithMessage(string.Format(ValidationMessages.MustBeStringWithLength, "{PropertyName}", length));
        }
        public static IRuleBuilderOptions<T, string> MaxStringLength<T>(this IRuleBuilder<T, string> ruleBuilder, int max)
        {
            return ruleBuilder
                .Must(r => r == null || r.Length <= max)
                .WithMessage(string.Format(ValidationMessages.StringLength, "{PropertyName}", max.ToString()));
        }

        public static IRuleBuilderOptions<T, string> MustBeNumberWithInRange<T>(this IRuleBuilder<T, string> ruleBuilder, int minRange, int maxRange, string message = null)
        {
            return ruleBuilder
                .Must(r => r.IsInt() && r.ToInt() >= minRange && r.ToInt() <= maxRange)
                .WithMessage(!string.IsNullOrWhiteSpace(message) ? message : string.Format(ValidationMessages.MustBeNumberWithInRange, "{PropertyName}", minRange, maxRange));
        }

        public static IRuleBuilderOptions<T, string> ValidDate<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(y => y.IsDateTimeWithFormat())
                .WithMessage(string.Format(ValidationMessages.MustBeValidDate, "{PropertyName}"));
        }

        public static IRuleBuilderOptions<T, string> NotFutureDate<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(y => y.IsDateTimeWithFormat() && y.ParseStringToDateTime() < DateTime.Now)
                .WithMessage(string.Format(ValidationMessages.DateNotinFuture, "{PropertyName}"));
        }

        public static IRuleBuilderOptions<T, string> MustBeInAcademicYearPattern<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(y => Regex.IsMatch(y, academicYearPattern))
                .WithMessage(string.Format(ValidationMessages.MustBeInFormat, "{PropertyName}", "YYYY/YY"));
        }

        public static IRuleBuilderOptions<T, string> MusBeValidAssessmentSeries<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(y => Regex.IsMatch(y, assessmentEntryFormat, RegexOptions.IgnoreCase));
        }

        public static IRuleBuilderOptions<T, string> MustBeNullOrEmpty<T>(this IRuleBuilder<T, string> ruleBuilder, string message = null)
        {
            return ruleBuilder
                .Must(r => string.IsNullOrWhiteSpace(r))
                .WithMessage(!string.IsNullOrWhiteSpace(message) ? message : string.Format(ValidationMessages.CannotHaveValue, "{PropertyName}"));
        }

        public static IRuleBuilderOptions<T, string> MustBeValidIndustryPlacementStatus<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(r => EnumExtensions.IsValidDisplayName<Models.IndustryPlacement.BulkProcess.IndustryPlacementStatus>(r));
        }
        public static IRuleBuilderOptions<T, string> MustBeValidSpecialConditionReason<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(r => r.Split(",").All(i =>  EnumExtensions.IsValidDisplayName<Models.IndustryPlacement.BulkProcess.IndustryPlacementSpecialConditionReason>(i)));
        }
    }
}
