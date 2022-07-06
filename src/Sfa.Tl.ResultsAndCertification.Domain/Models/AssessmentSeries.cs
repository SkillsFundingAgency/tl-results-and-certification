using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class AssessmentSeries : BaseEntity
    {
        public ComponentType ComponentType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RommEndDate { get; set; }
        public DateTime AppealEndDate { get; set; }
        public int? ResultCalculationYear { get; set; }
        public DateTime? ResultPublishDate {get;set;}
    }
}
