using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.ChangeRecordLink;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAssessmentSeriesDates
{
    public class AdminAssessmentSeriesDatesViewModel
    {
        public AdminAssessmentSeriesDatesCriteriaViewModel SearchCriteria { get; set; } = new AdminAssessmentSeriesDatesCriteriaViewModel();

        public IEnumerable<AdminAssessmentSeriesViewModel> Series { get; set; } = new List<AdminAssessmentSeriesViewModel>();

        public ChangeRecordModel NotificationDetailsLink { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminHome
        };

    }
}