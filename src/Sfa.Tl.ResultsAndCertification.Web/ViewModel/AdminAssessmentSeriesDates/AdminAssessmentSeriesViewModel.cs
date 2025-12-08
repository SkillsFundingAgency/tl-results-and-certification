using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAssessmentSeriesDates
{
    public class AdminAssessmentSeriesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ComponentType ComponentType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}