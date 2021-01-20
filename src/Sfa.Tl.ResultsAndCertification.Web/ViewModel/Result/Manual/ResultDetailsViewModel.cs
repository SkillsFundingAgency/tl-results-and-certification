using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ResultDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class ResultDetailsViewModel
    {
        private string PathwayResultActionText { get { return !string.IsNullOrWhiteSpace(PathwayResult) ? ResultDetailsContent.Change_Result_Action_Link_Text : ResultDetailsContent.Add_Result_Action_Link_Text; } }
        private string PathwayResultText { get { return string.Format(ResultDetailsContent.Grade_Label_Text, !string.IsNullOrWhiteSpace(PathwayResult) ? PathwayResult : ResultDetailsContent.Not_Specified_Text); } }

        private string PathwayAddResultRoute { get { return !string.IsNullOrWhiteSpace(PathwayResult) ? RouteConstants.ChangeCoreResult : RouteConstants.AddCoreResult; } }
        private Dictionary<string, string> PathwayResultRouteAttributes { get { return !string.IsNullOrWhiteSpace(PathwayResult) ? new Dictionary<string, string> { { Constants.ResultId, PathwayResultId.ToString() } } : new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }; } }

        public int ProfileId { get; set; }
        public long Uln { get; set; }
        public string Name { get; set; }
        public string ProviderDisplayName { get; set; }
        
        public string PathwayDisplayName { get; set; }
        public string PathwayAssessmentSeries { get; set; }
        
        public string SpecialismDisplayName { get; set; }

        public string PathwayResult { get; set; }
        public int PathwayResultId { get; set; }
        
        public RegistrationPathwayStatus PathwayStatus { get; set; }
        public bool IsCoreAssessmentEntryAdded { get { return !string.IsNullOrEmpty(PathwayAssessmentSeries); } }

        public SummaryItemModel SummaryCoreResult => new SummaryItemModel
        {
            Id = "coreresult",
            Title = ResultDetailsContent.Title_Result_Text,
            Value = PathwayAssessmentSeries, 
            AdditionalColumn = PathwayResultText, 
            ActionText = PathwayResultActionText,
            RouteName = PathwayAddResultRoute,
            HiddenActionText = ResultDetailsContent.Core_Result_Hidden_Text,
            RouteAttributes = PathwayResultRouteAttributes
        };

        public BreadcrumbModel Breadcrumb
        {
            get
            {
                return new BreadcrumbModel
                {
                    BreadcrumbItems = new List<BreadcrumbItem>
                    {
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Result_Dashboard, RouteName = RouteConstants.ResultsDashboard },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Results, RouteName = RouteConstants.SearchResults },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Learners_Results }
                    }
                };
            }
        }
    }
}
