using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class UcasDataAbbreviations
    {
        private static readonly Dictionary<string, string> _pathwayResultAbbreviations = new(StringComparer.InvariantCultureIgnoreCase)
        {
            { "A*", "A*" },
            { "A", "A" },
            { "B", "B" },
            { "C", "C" },
            { "D", "D" },
            { "E", "E" },
            { "Unclassified", "U" },
            { "X - no result", "X" },
            { "Q - pending result", "Q"}
        };

        private static readonly Dictionary<string, string> _specialismResultAbbreviations = new(StringComparer.InvariantCultureIgnoreCase)
        {
            { "Distinction", "D" },
            { "Merit", "M" },
            { "Pass", "P" },
            { "Unclassified", "U" },
            { "X - no result", "X" },
            { "Q - pending result", "Q"}
        };

        private static readonly Dictionary<string, string> _overallResultsAbbreviations = new(StringComparer.InvariantCultureIgnoreCase)
        {
            { "Distinction*", "D*" },
            { "Distinction", "D" },
            { "Merit", "M" },
            { "Pass", "P" },
            { "Partial achievement", "PA" },
            { "X - no result", "X" },
            { "Unclassified", "U" },
            { "Q - pending result", "Q"}
        };

        private static readonly Dictionary<string, string> _industryPlacementResultsAbbreviations = new(StringComparer.InvariantCultureIgnoreCase)
        {
            { "Completed", "P" },
            { "Completed with special consideration", "P" },
            { "Not completed", "PC" },
            { "Will not complete", "NC" }
        };

        public static string GetAbbreviatedResult(UcasResultType ucasResultType, string result)
        {
            if (ucasResultType != UcasResultType.OverallResult && string.IsNullOrWhiteSpace(result))
                return string.Empty;

            bool hasValue = false;
            string abbreviatedResult = null;

            switch (ucasResultType)
            {
                case UcasResultType.PathwayResult:
                    hasValue = _pathwayResultAbbreviations.TryGetValue(result, out abbreviatedResult);
                    break;
                case UcasResultType.SpecialismResult:
                    hasValue = _specialismResultAbbreviations.TryGetValue(result, out abbreviatedResult);
                    break;
                case UcasResultType.OverallResult:
                    hasValue = _overallResultsAbbreviations.TryGetValue(result, out abbreviatedResult);
                    break;
                case UcasResultType.IndustryPlacement:
                    hasValue = _industryPlacementResultsAbbreviations.TryGetValue(result, out abbreviatedResult);
                    break;
            }

            if (hasValue)
                return abbreviatedResult;
            else
                throw new ApplicationException($"{ucasResultType} abbreviated result cannot be null");
        }
    }
}
