using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Domain
{
    public static class Extensions
    {
        public static bool IsActive(this TqRegistrationPathway registrationPathway)
            => registrationPathway.Status == RegistrationPathwayStatus.Active;

        public static IEnumerable<TqRegistrationPathway> WhereActive(this IEnumerable<TqRegistrationPathway> registrationPathways)
            => registrationPathways.Where(p => p.IsActive());

        public static bool IsActive(this TqPathwayAssessment pathwayAssessment)
            => pathwayAssessment.IsOptedin && !pathwayAssessment.EndDate.HasValue;

        public static IEnumerable<TqPathwayAssessment> WhereActive(this IEnumerable<TqPathwayAssessment> pathwayAssessments)
            => pathwayAssessments.Where(p => p.IsActive());

        public static bool IsActive(this TqPathwayResult pathwayResult)
            => pathwayResult.IsOptedin && !pathwayResult.EndDate.HasValue;

        public static IEnumerable<TqPathwayResult> WhereActive(this IEnumerable<TqPathwayResult> pathwayResults)
            => pathwayResults.Where(p => p.IsActive());

        public static bool IsActive(this TqRegistrationSpecialism registrationSpecialism)
            => registrationSpecialism.IsOptedin && !registrationSpecialism.EndDate.HasValue;

        public static IEnumerable<TqRegistrationSpecialism> WhereActive(this IEnumerable<TqRegistrationSpecialism> registrationSpecialisms)
            => registrationSpecialisms.Where(p => p.IsActive());

        public static bool IsActive(this TqSpecialismAssessment specialismAssessment)
            => specialismAssessment.IsOptedin && !specialismAssessment.EndDate.HasValue;

        public static IEnumerable<TqSpecialismAssessment> WhereActive(this IEnumerable<TqSpecialismAssessment> specialismAssessments)
            => specialismAssessments.Where(p => p.IsActive());

        public static bool IsActive(this TqSpecialismResult specialismResult)
            => specialismResult.IsOptedin && !specialismResult.EndDate.HasValue;

        public static IEnumerable<TqSpecialismResult> WhereActive(this IEnumerable<TqSpecialismResult> specialismResults)
            => specialismResults.Where(p => p.IsActive());
    }
}