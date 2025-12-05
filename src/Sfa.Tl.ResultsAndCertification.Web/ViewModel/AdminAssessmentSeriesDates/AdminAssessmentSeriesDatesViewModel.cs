using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAssessmentSeriesDates
{
    public class AdminAssessmentSeriesDatesViewModel
    {
        public AdminAssessmentSeriesDatesCriteriaViewModel SearchCriteria { get; set; } = new AdminAssessmentSeriesDatesCriteriaViewModel();

        public List<AdminAssessmentSeriesDateDetailsViewModel> Series { get; set; } = new List<AdminAssessmentSeriesDateDetailsViewModel>();

        public int TotalRecords { get; set; }

        public bool ContainsResults
            => !Series.IsNullOrEmpty() && Series.Count > 0;

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminHome
        };
    }
}