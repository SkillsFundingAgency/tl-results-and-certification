using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using ResultDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ResultDetails;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class ResultDetailsViewModel : ResultsBaseViewModel
    {
        public ResultDetailsViewModel()
        {
            UlnLabel = ResultDetailsContent.Title_Uln_Text;
            LearnerNameLabel = ResultDetailsContent.Title_Name_Text;
            DateofBirthLabel = ResultDetailsContent.Title_DateofBirth_Text;
            ProviderNameLabel = ResultDetailsContent.Title_Provider_Text;
            TlevelTitleLabel = ResultDetailsContent.Title_TLevel_Text;
        }

        public int ProfileId { get; set; }
        public string PathwayDisplayName { get; set; }
        public string PathwayAssessmentSeries { get; set; }
        public int PathwayAssessmentId { get; set; }
        public string PathwayResult { get; set; }
        public int PathwayResultId { get; set; }
        public RegistrationPathwayStatus PathwayStatus { get; set; }
        public PrsStatus? PathwayPrsStatus { get; set; }
        public DateTime? AppealEndDate { get; set; }

        public bool IsValid { get { return PathwayAssessmentId != 0; } }
        public bool IsValidPathwayPrsStatus => PathwayPrsStatus.HasValue && PathwayPrsStatus != PrsStatus.NotSpecified;
        public bool IsCoreAssessmentEntryAdded { get { return !string.IsNullOrEmpty(PathwayAssessmentSeries); } }
        
        public SummaryItemModel SummaryAssessmentSeries => new SummaryItemModel
        {
            Id = "assessmentperiod",
            Title = ResultDetailsContent.Title_Assessment_Series,
            Value = PathwayAssessmentSeries,
            RenderEmptyRowForValue2 = IsValidPathwayPrsStatus,
            RenderActionColumn = !IsValidPathwayPrsStatus
        };

        public SummaryItemModel SummaryPathwayGrade
        {
            get
            {
                return IsValidPathwayPrsStatus
                    ? new SummaryItemModel
                    {
                        Id = "pathwaygrade",
                        Title = ResultDetailsContent.Title_Pathway_Grade,
                        Value = PathwayResultText,
                        Value2 = CommonHelper.GetPrsStatusDisplayText(PathwayPrsStatus, AppealEndDate),
                        Value2CustomCssClass = Constants.TagFloatRightClassName,
                    }
                    : new SummaryItemModel
                        {
                            Id = "pathwaygrade",
                            Title = ResultDetailsContent.Title_Pathway_Grade,
                            Value = PathwayResultText,
                            RenderActionColumn = true,
                            ActionText = PathwayResultActionText,
                            RouteName = PathwayAddResultRoute,
                            HiddenValueText = ResultDetailsContent.Hidden_Value_Text_For,
                            HiddenActionText = PathwayActionHiddenText,
                            RouteAttributes = PathwayResultRouteAttributes
                        };
            }
        }
            
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

        private string PathwayResultActionText { get { return !string.IsNullOrWhiteSpace(PathwayResult) ? ResultDetailsContent.Change_Result_Action_Link_Text : ResultDetailsContent.Add_Result_Action_Link_Text; } }
        private string PathwayResultText { get { return !string.IsNullOrWhiteSpace(PathwayResult) ? PathwayResult : ResultDetailsContent.Not_Received_Text; } }
        private string PathwayAddResultRoute { get { return !string.IsNullOrWhiteSpace(PathwayResult) ? RouteConstants.ChangeCoreResult : RouteConstants.AddCoreResult; } }
        private Dictionary<string, string> PathwayResultRouteAttributes { get { return new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.AssessmentId, PathwayAssessmentId.ToString() } }; } }
        private string PathwayActionHiddenText { get { return string.IsNullOrWhiteSpace(PathwayResult) ? ResultDetailsContent.Hidden_Action_Text_For_Core : ResultDetailsContent.Hidden_Action_Text_Core; } }

    }
}
