using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Strategies
{
    public class OverallGradeStrategy2020 : IOverallGradeStrategy
    {
        private readonly IEnumerable<TlLookup> _overallResultLookup;
        private readonly IEnumerable<OverallGradeLookup> _overallGradeLookup;

        public OverallGradeStrategy2020(IEnumerable<TlLookup> overallResultLookup, IEnumerable<OverallGradeLookup> overallGradeLookup)
        {
            _overallResultLookup = overallResultLookup;
            _overallGradeLookup = overallGradeLookup;
        }

        public string GetOverAllGrade(int tlPathwayId, int? pathwayGradeId, int? speciailsmGradeId, IndustryPlacementStatus ipStatus)
        {
            // Q - pending result
            var isPathwayGradeQpending = IsComponentGradeWithCode(_overallResultLookup, pathwayGradeId, Constants.PathwayComponentGradeQpendingResultCode);
            var isSpecialismGradeQpending = IsComponentGradeWithCode(_overallResultLookup, speciailsmGradeId, Constants.SpecialismComponentGradeQpendingResultCode);

            // Unclassified result
            var isPathwayGradeUnclassified = IsComponentGradeWithCode(_overallResultLookup, pathwayGradeId, Constants.PathwayComponentGradeUnclassifiedCode);
            var isSpecialismGradeUnclassified = IsComponentGradeWithCode(_overallResultLookup, speciailsmGradeId, Constants.SpecialismComponentGradeUnclassifiedCode);

            // X - No result
            var isPathwayGradeXNoResult = IsComponentGradeWithCode(_overallResultLookup, pathwayGradeId, Constants.PathwayComponentGradeXNoResultCode);
            var isSpecialismGradeXNoResult = IsComponentGradeWithCode(_overallResultLookup, speciailsmGradeId, Constants.SpecialismComponentGradeXNoResultCode);

            var overallResultQpending = _overallResultLookup.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultQpendingCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var overallResultUnClassified = _overallResultLookup.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultUnclassifiedCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var overallResultXNoResult = _overallResultLookup.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultXNoResultCode, StringComparison.InvariantCultureIgnoreCase))?.Value;
            var overallResultPartialAchievement = _overallResultLookup.FirstOrDefault(o => o.Code.Equals(Constants.OverallResultPartialAchievementCode, StringComparison.InvariantCultureIgnoreCase))?.Value;

            if (isPathwayGradeQpending || isSpecialismGradeQpending)
            {
                return overallResultQpending;
            }
            else if (ipStatus == IndustryPlacementStatus.Completed || ipStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration)
            {
                if (pathwayGradeId.HasValue && speciailsmGradeId.HasValue &&
                    !isPathwayGradeUnclassified && !isSpecialismGradeUnclassified &&
                    !isPathwayGradeXNoResult && !isSpecialismGradeXNoResult)
                {
                    var overallGrade = _overallGradeLookup.FirstOrDefault(o => o.TlPathwayId == tlPathwayId && o.TlLookupCoreGradeId == pathwayGradeId && o.TlLookupSpecialismGradeId == speciailsmGradeId);
                    return overallGrade?.TlLookupOverallGrade?.Value;
                }
                else
                {
                    return overallResultPartialAchievement;
                }
            }
            else
            {
                // if ipStatus is NotSpecified || ipStatus is NotCompleted || ipStatus is WillNotComplete then apply below logic

                if (pathwayGradeId.HasValue && speciailsmGradeId.HasValue)
                {
                    if ((isPathwayGradeUnclassified && isSpecialismGradeUnclassified) ||
                             (isPathwayGradeUnclassified && isSpecialismGradeXNoResult) ||
                             (isPathwayGradeXNoResult && isSpecialismGradeUnclassified))
                    {
                        return overallResultUnClassified;
                    }
                    else if (isPathwayGradeXNoResult && isSpecialismGradeXNoResult)
                    {
                        return overallResultXNoResult;
                    }
                    else
                    {
                        return overallResultPartialAchievement;
                    }
                }
                else if (pathwayGradeId.HasValue && !speciailsmGradeId.HasValue)
                {
                    if (isPathwayGradeUnclassified)
                    {
                        return overallResultUnClassified;
                    }
                    else if (isPathwayGradeXNoResult)
                    {
                        return overallResultXNoResult;
                    }
                    else
                    {
                        return overallResultPartialAchievement;
                    }
                }
                else if (!pathwayGradeId.HasValue && speciailsmGradeId.HasValue)
                {
                    if (isSpecialismGradeUnclassified)
                    {
                        return overallResultUnClassified;
                    }
                    else if (isSpecialismGradeXNoResult)
                    {
                        return overallResultXNoResult;
                    }
                    else
                    {
                        return overallResultPartialAchievement;
                    }
                }
                else
                {
                    // if pathwayGradeId is null && speciailsmGradeId is null return X - no result
                    return overallResultXNoResult;
                }
            }
        }

        private bool IsComponentGradeWithCode(IEnumerable<TlLookup> overallResultLookup, int? gradeId, string gradeCode)
        {
            return gradeId.HasValue ? overallResultLookup.Any(o => o.Id == gradeId.Value && o.Code.Equals(gradeCode, StringComparison.InvariantCultureIgnoreCase)) : false;
        }
    }
}