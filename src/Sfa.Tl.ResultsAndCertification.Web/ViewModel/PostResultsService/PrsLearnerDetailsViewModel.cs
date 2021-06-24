using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using PrsLearnerDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsLearnerDetails;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsLearnerDetailsViewModel : PrsBaseViewModel
    {
        public PrsLearnerDetailsViewModel()
        {
            UlnLabel = PrsLearnerDetailsContent.Title_Uln_Text;
            LearnerNameLabel = PrsLearnerDetailsContent.Title_Name_Text;
            DateofBirthLabel = PrsLearnerDetailsContent.Title_DateofBirth_Text;
            ProviderNameLabel = PrsLearnerDetailsContent.Title_Provider_Text;
            TlevelTitleLabel = PrsLearnerDetailsContent.Title_TLevel_Text;
        }

        public bool IsValid { get { return Status == RegistrationPathwayStatus.Active; } }

        public int ProfileId { get; set; }
        public string PathwayTitle { get; set; }
        public RegistrationPathwayStatus Status { get; set; }

        public int PathwayAssessmentId { get; set; }
        public string PathwayAssessmentSeries { get; set; }
        public int PathwayResultId { get; set; }
        public string PathwayGrade { get; set; }
        public string PathwayGradeLastUpdatedOn { get; set; }
        public string PathwayGradeLastUpdatedBy { get; set; }

        public SummaryItemModel SummaryAssessmentSeries => new SummaryItemModel
        {
            Id = "assessmentperiod",
            Title = PrsLearnerDetailsContent.Title_Assessment_Series,
            Value = PathwayAssessmentSeries,
            NeedBorderBottomLine = true
        };
        public SummaryItemModel SummaryPathwayGrade => new SummaryItemModel
        {
            Id = "pathwaygrade",
            Title = PrsLearnerDetailsContent.Title_Pathway_Grade,
            Value = PathwayGrade,
            NeedBorderBottomLine = true,

            ActionText = PrsLearnerDetailsContent.Action_Link_Update,
            RouteName = RouteConstants.PrsAppealCoreGrade,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.AssessmentId, PathwayAssessmentId.ToString() }, { Constants.ResultId, PathwayResultId.ToString() } },
            HiddenActionText = PrsLearnerDetailsContent.Hidden_Action_Text_Grade
        };

        public SummaryItemModel SummaryPathwayGradeLastUpdatedOn => new SummaryItemModel
        {
            Id = "pathwaygradeupdatedon",
            Title = PrsLearnerDetailsContent.Title_Pathway_Grade_LastUpdatedOn,
            Value = PathwayGradeLastUpdatedOn,
            NeedBorderBottomLine = true
        };

        public SummaryItemModel SummaryPathwayGradeLastUpdatedBy => new SummaryItemModel
        {
            Id = "pathwaygradeupdatedby",
            Title = PrsLearnerDetailsContent.Title_Pathway_Grade_LastUpdatedBy,
            Value = PathwayGradeLastUpdatedBy,
            NeedBorderBottomLine = true
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
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.StartReviewsAndAppeals, RouteName = RouteConstants.StartReviewsAndAppeals },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Learner, RouteName = RouteConstants.PrsSearchLearner },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Prs_Learner_Component_Grade_Status }
                    }
                };
            }
        }
    }
}