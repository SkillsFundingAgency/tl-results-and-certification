using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class OverallResult : BaseEntity
    {
        public int TqRegistrationPathwayId { get; set; }
        public string Details { get; set; }
        public string ResultAwarded { get; set; }
        public CalculationStatus CalculationStatus { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? PrintAvailableFrom { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual TqRegistrationPathway TqRegistrationPathway { get; set; }
    }
}
