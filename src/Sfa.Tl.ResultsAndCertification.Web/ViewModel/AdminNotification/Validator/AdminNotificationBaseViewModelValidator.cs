using FluentValidation;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.CsvHelper.Helpers.Extensions;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminNotification;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminNotification.Validator
{
    public class AdminNotificationBaseViewModelValidator : AbstractValidator<AdminNotificationBaseViewModel>
    {
        private const int TitleMaxLength = 255;

        public AdminNotificationBaseViewModelValidator()
        {
            // Title
            RuleFor(r => r.Title)
                .Required()
                .WithMessage(AdminNotificationBase.Validation_Title_Blank_Text);

            RuleFor(r => r.Title)
                .MaximumLength(TitleMaxLength)
                .WithMessage(AdminNotificationBase.Validation_Title_Max_Length);

            // Content
            RuleFor(r => r.Content)
                .Required()
                .WithMessage(AdminNotificationBase.Validation_Content_Blank_Text);

            // Target
            RuleFor(r => r.Target)
                .Must(t => t != NotificationTarget.NotSpecified)
                .WithMessage(AdminNotificationBase.Validation_Who_Needs_To_See_It_Not_Selected);

            // Start day
            RuleFor(r => r.StartDay)
                .Required()
                .WithMessage(AdminNotificationBase.Validation_Start_Date_Day_When_Blank);

            // Start month
            RuleFor(r => r.StartMonth)
                .Required()
                .WithMessage(AdminNotificationBase.Validation_Start_Date_Month_When_Blank);

            // Start year
            RuleFor(r => r.StartYear)
                .Required()
                .WithMessage(AdminNotificationBase.Validation_Start_Date_Year_When_Blank);

            // End day
            RuleFor(r => r.EndDay)
                .Required()
                .WithMessage(AdminNotificationBase.Validation_End_Date_Day_When_Blank);

            // End month
            RuleFor(r => r.EndMonth)
                .Required()
                .WithMessage(AdminNotificationBase.Validation_End_Date_Month_When_Blank);

            // End year
            RuleFor(r => r.EndYear)
                .Required()
                .WithMessage(AdminNotificationBase.Validation_End_Date_Year_When_Blank);

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = startDate;

            // Start date
            RuleFor(r => r.StartDate)
                .Must(d => IsValidDate(d, out startDate))
                .WithMessage(AdminNotificationBase.Validation_Start_Date_When_Invalid_Text)
                .When(r => !string.IsNullOrWhiteSpace(r.StartDay) && !string.IsNullOrWhiteSpace(r.StartMonth) && !string.IsNullOrWhiteSpace(r.StartYear));

            // End date
            RuleFor(r => r.EndDate)
                .Must(d => IsValidDate(d, out endDate))
                .WithMessage(AdminNotificationBase.Validation_End_Date_When_Invalid_Text)
                .When(r => !string.IsNullOrWhiteSpace(r.EndDay) && !string.IsNullOrWhiteSpace(r.EndMonth) && !string.IsNullOrWhiteSpace(r.EndYear));

            RuleFor(r => r.EndDate)
                .Must(d => endDate >= startDate)
                .WithMessage(AdminNotificationBase.Validation_End_Date_Must_Be_After_The_Start_Date)
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