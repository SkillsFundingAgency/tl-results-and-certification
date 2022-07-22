using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class UcasDataAbbreviations
    {
        private readonly Dictionary<string, string> _pathwayResultAbbreviations = new(StringComparer.InvariantCultureIgnoreCase)
        {
            { "A*", "A*" },
            { "A", "A" },
            { "B", "B" },
            { "C", "C" },
            { "D", "D" },
            { "E", "E" },
            { "Unclassified", "U" }
        };

        private readonly Dictionary<string, string> _specialismResultAbbreviations = new(StringComparer.InvariantCultureIgnoreCase)
        {
            { "Distinction", "D" },
            { "Merit", "M" },
            { "Pass", "P" },
            { "Unclassified", "U" }
        };

        private readonly Dictionary<string, string> _overallResultsAbbreviations = new(StringComparer.InvariantCultureIgnoreCase)
        {
            { "Distinction*", "D*" },
            { "Distinction", "D" },
            { "Merit", "M" },
            { "Pass", "P" },
            { "Partial achievement", "PA" },
            { "X – no result", "X" },
            { "Unclassified", "U" }
        };

        public string GetAbbreviatedPathwayResult(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
                return string.Empty;

            var hasValue = _pathwayResultAbbreviations.TryGetValue(result, out string abbrevatedResult);

            if (hasValue)
                return abbrevatedResult;
            else
                throw new ApplicationException("Pathway abbreviated result cannot be null");
        }

        public string GetAbbreviatedSpecialismResult(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
                return string.Empty;

            var hasValue = _specialismResultAbbreviations.TryGetValue(result, out string abbrevatedResult);

            if (hasValue)
                return abbrevatedResult;
            else
                throw new ApplicationException("Specialism abbreviated result cannot be null");
        }

        public string GetAbbreviatedOverallResult(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
                return string.Empty;

            var hasValue = _overallResultsAbbreviations.TryGetValue(result, out string abbrevatedResult);

            if (hasValue)
                return abbrevatedResult;
            else
                throw new ApplicationException("Overall abbreviated result cannot be null");
        }
    }
}
