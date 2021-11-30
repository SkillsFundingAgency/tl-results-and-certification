using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class PathwayAssessmentViewModel
    {
        public PathwayAssessmentViewModel()
        {
            Results = new List<ResultViewModel>();
        }

        public int? AssessmentId { get; set; }
        public int? SeriesId { get; set; }
        public string SeriesName { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }

        public List<ResultViewModel> Results { get; set; }
    }
}
