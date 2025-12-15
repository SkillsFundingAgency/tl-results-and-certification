using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminAssessmentSeriesDates;
using Sfa.Tl.ResultsAndCertification.Web.Content.Tlevel;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Pagination;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAssessmentSeriesDates
{
    public class AdminAssessmentSeriesDatesViewModel
    {
        public AdminAssessmentSeriesDatesCriteriaViewModel SearchCriteria { get; set; } = new AdminAssessmentSeriesDatesCriteriaViewModel();

        public IList<AdminAssessmentSeriesViewModel> Series { get; set; } = new List<AdminAssessmentSeriesViewModel>();

        public PagerViewModel PagerInfo { get; set; }

        public int TotalRecords { get; set; }

        public bool ContainsResults
          => !Series.IsNullOrEmpty() && Series.Count > 0;

        public bool ContainsMultiplePages
            => ContainsResults && Pagination?.PagerInfo?.TotalPages > 1;

        public PaginationModel Pagination => new()
        {
            PagerInfo = PagerInfo,
            RouteName = RouteConstants.SearchAssessmentSeriesDates,
            PaginationSummary = AdminAssessmentSeriesDateDetails.PaginationSummary_Text
        };

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminHome
        };
    }
}