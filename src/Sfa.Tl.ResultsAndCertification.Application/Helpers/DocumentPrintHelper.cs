using System.Globalization;

namespace Sfa.Tl.ResultsAndCertification.Application.Helpers
{
    /// <summary>
    /// return formatted learner name. Convert name into TitleCase if first/last name is in either uppercase or lowercase.
    /// </summary>
    public static class DocumentPrintHelper
    {
        public static string FormatLearnerName(string firstName, string lastName)
        {
            return $"{firstName.ApplyTitleCase()} {lastName.ApplyTitleCase()}";
        }

        private static string ApplyTitleCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (value.Trim().Equals(value.Trim().ToUpper()) || value.Trim().Equals(value.Trim().ToLower()))
                return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.ToLower());

            return value;
        }
    }
}
