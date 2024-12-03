using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminBanner;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminBanner.Validator
{
    public class AdminBannerBaseViewModelValidator : AbstractValidator<AdminBannerBaseViewModel>
    {
        private const int TitleMaxLength = 255;
        private const int ContentMaxLength = 500;

        public AdminBannerBaseViewModelValidator()
        {
            // Title
            RuleFor(r => r.Title)
                .Required()
                .WithMessage(AdminBannerBase.Validation_Title_Blank_Text);

            RuleFor(r => r.Title)
                .MaximumLength(TitleMaxLength)
                .WithMessage(AdminBannerBase.Validation_Title_Max_Length);

            // Content
            RuleFor(r => r.Content)
                .Required()
                .WithMessage(AdminBannerBase.Validation_Content_Blank_Text);

            RuleFor(r => r.Content)
                .MaximumLength(ContentMaxLength)
                .WithMessage(AdminBannerBase.Validation_Content_Max_Length);

            // Target
            RuleFor(r => r.Target)
                .Must(t => t != BannerTarget.NotSpecified)
                .WithMessage(AdminBannerBase.Validation_Who_Needs_To_See_It_Not_Selected);

            // Start day
            RuleFor(r => r.StartDay)
                .Required()
                .WithMessage(AdminBannerBase.Validation_Start_Date_Day_When_Blank);

            // Start month
            RuleFor(r => r.StartMonth)
                .Required()
                .WithMessage(AdminBannerBase.Validation_Start_Date_Month_When_Blank);

            // Start year
            RuleFor(r => r.StartYear)
                .Required()
                .WithMessage(AdminBannerBase.Validation_Start_Date_Year_When_Blank);

            // End day
            RuleFor(r => r.EndDay)
                .Required()
                .WithMessage(AdminBannerBase.Validation_End_Date_Day_When_Blank);

            // End month
            RuleFor(r => r.EndMonth)
                .Required()
                .WithMessage(AdminBannerBase.Validation_End_Date_Month_When_Blank);

            // End year
            RuleFor(r => r.EndYear)
                .Required()
                .WithMessage(AdminBannerBase.Validation_End_Date_Year_When_Blank);

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = startDate;

            // Start date
            RuleFor(r => r.StartDate)
                .Must(d => IsValidDate(d, out startDate))
                .WithMessage(AdminBannerBase.Validation_Start_Date_When_Invalid_Text)
                .When(r => !string.IsNullOrWhiteSpace(r.StartDay) && !string.IsNullOrWhiteSpace(r.StartMonth) && !string.IsNullOrWhiteSpace(r.StartYear));

            // End date
            RuleFor(r => r.EndDate)
                .Must(d => IsValidDate(d, out endDate))
                .WithMessage(AdminBannerBase.Validation_End_Date_When_Invalid_Text)
                .When(r => !string.IsNullOrWhiteSpace(r.EndDay) && !string.IsNullOrWhiteSpace(r.EndMonth) && !string.IsNullOrWhiteSpace(r.EndYear));

            RuleFor(r => r.EndDate)
                .Must(d => endDate > startDate)
                .WithMessage(AdminBannerBase.Validation_End_Date_Must_Be_After_The_Start_Date)
                .When(r => IsValidDate(r.StartDate, out startDate) && IsValidDate(r.EndDate, out endDate));
        }

        private static bool IsValidDate(string date, out DateTime parsed)
        {
            parsed = DateTime.MinValue;

            string[] tokens = date.Split("/");

            string year = tokens[0];
            string month = tokens[1];
            string day = tokens[2];

            bool allNumeric = IsNumericText(year, out int parsedYear) & IsNumericText(month, out int parsedMonth) & IsNumericText(day, out int parsedDay);
            if (!allNumeric)
            {
                return false;
            }

            bool validNumbers = IsBetween(parsedYear, 1000, 9999) && IsBetween(parsedMonth, 1, 12) && IsBetween(parsedDay, 1, 31);
            if (!validNumbers)
            {
                return false;
            }

            try
            {
                parsed = new(parsedYear, parsedMonth, parsedDay);
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            return true;
        }

        private static bool IsNumericText(string value, out int parsed)
            => int.TryParse(value, out parsed);

        private static bool IsBetween(int number, int from, int to)
            => number >= from && number <= to;
    }
}