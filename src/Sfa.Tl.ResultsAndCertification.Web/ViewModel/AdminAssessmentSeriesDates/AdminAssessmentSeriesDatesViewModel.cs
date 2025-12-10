using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAssessmentSeriesDates
{
    public class AdminAssessmentSeriesDatesViewModel
    {
        public AdminAssessmentSeriesDatesCriteriaViewModel SearchCriteria { get; set; } = new AdminAssessmentSeriesDatesCriteriaViewModel();

        public IEnumerable<AdminAssessmentSeriesViewModel> Series { get; set; } = new List<AdminAssessmentSeriesViewModel>();

        public string TotalRecords => Series != null ? Series.Count().ToString() : "0";

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminHome
        };

    }
}