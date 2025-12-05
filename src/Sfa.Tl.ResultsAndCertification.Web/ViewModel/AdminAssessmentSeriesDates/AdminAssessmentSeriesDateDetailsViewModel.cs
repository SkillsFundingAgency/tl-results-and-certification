using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAssessmentSeriesDates
{
    public class AdminAssessmentSeriesDateDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ComponentType ComponentType { get; set; }
        public string ResultsYear { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RommEndDate { get; set; }
        public DateTime AppealEndDate { get; set; }
        public DateTime ResultPublishDate { get; set; }
        public DateTime PrintAvailableDate { get; set; }

    }
}