using System.Globalization;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Helpers
{
    public static class DocumentPrintHelper
    {
        public static string FormatLearnerName(string firstName, string lastName)
        {
            return $"{firstName.ApplyTitleCase()} {lastName.ApplyTitleCase()}";
        }

        /// <summary>
        /// Convert ToTitleCase if input string is in uppercase.
        /// </summary>
        public static string ApplyTitleCase(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (value.Replace(" ", string.Empty).All(c => char.IsUpper(c)))
                return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.ToLower());

            return value;
        }
    }
}
