using System;

namespace Sfa.Tl.ResultsAndCertification.Domain.Models
{
    public partial class AcademicYear : BaseEntity
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}