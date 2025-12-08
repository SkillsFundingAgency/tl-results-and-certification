using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminAssessmentSeriesDates
{
    public class AdminAssessmentSeriesDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SummaryItemModel ComponentType { get; set; }
        public SummaryItemModel SummaryResultCalculationYear { get; set; }
        public SummaryItemModel SummaryResultsYear { get; set; }
        public SummaryItemModel SummaryStartDate { get; set; }
        public SummaryItemModel SummaryEndDate { get; set; }
        public SummaryItemModel SummaryRommEndDate { get; set; }
        public SummaryItemModel SummaryAppealEndDate { get; set; }
        public SummaryItemModel SummaryResultPublishDate { get; set; }
        public SummaryItemModel SummaryPrintAvailableDate { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminAssessmentSeriesDateDetails
        };
    }
}