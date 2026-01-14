using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminAssessmentSeriesDates
{
    public class GetAssessmentSeriesDatesDetailsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ComponentType ComponentType { get; set; }
        public int? ResultCalculationYear { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RommEndDate { get; set; }
        public DateTime AppealEndDate { get; set; }
        public DateTime? ResultPublishDate { get; set; }
        public DateTime? PrintAvailableDate { get; set; }
    }
}
