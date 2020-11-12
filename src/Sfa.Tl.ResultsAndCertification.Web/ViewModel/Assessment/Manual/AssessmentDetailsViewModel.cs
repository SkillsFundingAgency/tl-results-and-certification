using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using AssessmentDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AssessmentDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class AssessmentDetailsViewModel
    {
        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }

        public string ProviderDisplayName { get; set; }
        public string PathwayDisplayName { get; set; }
        public string PathwayAssessmentSeries { get; set; }

        public string SpecialismDisplayName { get; set; }
        public string SpecialismAssessmentSeries { get; set; }

        public RegistrationPathwayStatus PathwayStatus { get; set; }

        private string PathwayAssessmentActionText { get { return !string.IsNullOrWhiteSpace(PathwayDisplayName) ? AssessmentDetailsContent.Change_Action_Link_Text : null; } }
        private string SpecialismAssessmentActionText { get { return !string.IsNullOrWhiteSpace(SpecialismDisplayName) ? AssessmentDetailsContent.Change_Action_Link_Text : null; } }

        private string PathwayAssessmentSeriesText { get { return !string.IsNullOrWhiteSpace(PathwayAssessmentSeries) ? PathwayAssessmentSeries : AssessmentDetailsContent.Not_Specified_Text; } }
        private string SpecialismAssessmentSeriesText { get { return !string.IsNullOrWhiteSpace(SpecialismAssessmentSeries) ? SpecialismAssessmentSeries : AssessmentDetailsContent.Not_Specified_Text; } }

        public SummaryItemModel SummaryCoreAssessmentEntry => new SummaryItemModel { Id = "coreassessmententry", Title = AssessmentDetailsContent.Title_Assessment_Entry_Text, Value = PathwayAssessmentSeriesText, ActionText = PathwayAssessmentActionText, HiddenActionText = AssessmentDetailsContent.Change_Core_Assessment_Entry_Hidden_Text };
        public SummaryItemModel SummarySpecialismAssessmentEntry => new SummaryItemModel { Id = "specialismassessmententry", Title = AssessmentDetailsContent.Title_Assessment_Entry_Text, Value = SpecialismAssessmentSeriesText, ActionText = SpecialismAssessmentActionText, HiddenActionText = AssessmentDetailsContent.Change_Specialism_Assessment_Entry_Hidden_Text };

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Assessment_Dashboard, RouteName = RouteConstants.AssessmentDashboard },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Assessments, RouteName = RouteConstants.SearchAssessments },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Learners_Assessment_entries }
                    }
                };
            }
        }
    }
}
