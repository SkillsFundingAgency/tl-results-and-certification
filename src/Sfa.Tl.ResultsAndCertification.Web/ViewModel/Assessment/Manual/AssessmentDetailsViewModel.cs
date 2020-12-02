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
        private string PathwayAssessmentActionText { get { return !string.IsNullOrWhiteSpace(PathwayAssessmentSeries) ? AssessmentDetailsContent.Remove_Entry_Action_Link_Text : AssessmentDetailsContent.Add_Entry_Action_Link_Text; } }
        private string SpecialismAssessmentActionText { get { return null; } }

        private string PathwayAssessmentSeriesText { get { return !string.IsNullOrWhiteSpace(PathwayAssessmentSeries) ? PathwayAssessmentSeries : AssessmentDetailsContent.Not_Specified_Text; } }
        private string SpecialismAssessmentSeriesText { get { return AssessmentDetailsContent.Available_After_Autumn2021; } }

        private string PathwayAddAssessmentRoute { get { return !string.IsNullOrWhiteSpace(PathwayAssessmentSeries) ? RouteConstants.RemoveCoreAssessmentEntry : RouteConstants.AddCoreAssessmentSeries; } }

        private Dictionary<string, string> PathwayAssessmentRouteAttributes { get { return !string.IsNullOrWhiteSpace(PathwayAssessmentSeries) ? new Dictionary<string, string> { { Constants.AssessmentId, PathwayAssessmentId.ToString() } } : new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } } ; } }

        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }

        public string ProviderDisplayName { get; set; }
        public string PathwayDisplayName { get; set; }
        public string PathwayAssessmentSeries { get; set; }
        public int PathwayAssessmentId { get; set; }

        public string SpecialismDisplayName { get; set; }
        public string SpecialismAssessmentSeries { get; set; }

        public RegistrationPathwayStatus PathwayStatus { get; set; }

        public SummaryItemModel SummaryCoreAssessmentEntry => new SummaryItemModel { Id = "coreassessmententry", Title = AssessmentDetailsContent.Title_Assessment_Entry_Text, Value = PathwayAssessmentSeriesText, ActionText = PathwayAssessmentActionText, HiddenActionText = AssessmentDetailsContent.Core_Assessment_Entry_Hidden_Text, RouteName = PathwayAddAssessmentRoute, RouteAttributes = PathwayAssessmentRouteAttributes };
        public SummaryItemModel SummarySpecialismAssessmentEntry => new SummaryItemModel { Id = "specialismassessmententry", Title = AssessmentDetailsContent.Title_Assessment_Entry_Text, Value = SpecialismAssessmentSeriesText, ActionText = SpecialismAssessmentActionText, HiddenActionText = AssessmentDetailsContent.Specialism_Assessment_Entry_Hidden_Text };

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
