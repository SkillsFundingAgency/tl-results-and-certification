using Sfa.Tl.ResultsAndCertification.Common.Extensions;
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

        public bool IsValid { get { return true; } } // TODO: 

        public string PathwayName { get; set; } // TODO: Ukprn need to be amended. 
        public string AssessmentPeriod { get; set; }
        public string PathwayGrade { get; set; }
        public System.DateTime PathwayGradeLastUpdatedOn { get; set; } // TODO: String move convertion to mapper 
        public string PathwayGradeLastUpdatedBy { get; set; }

        public SummaryItemModel SummaryAssessmentPeriod => new SummaryItemModel
        {
            Id = "assessmentperiod",
            Title = PrsLearnerDetailsContent.Title_Assessment_Period,
            Value = AssessmentPeriod,
            NeedBorderBottomLine = true 
        };
        public SummaryItemModel SummaryPathwayGrade => new SummaryItemModel
        {
            Id = "pathwaygrade",
            Title = PrsLearnerDetailsContent.Title_Pathway_Grade,
            Value = PathwayGrade,
            NeedBorderBottomLine = true
        };

        public SummaryItemModel SummaryPathwayGradeLastUpdatedOn => new SummaryItemModel
        {
            Id = "pathwaygradeupdatedon",
            Title = PrsLearnerDetailsContent.Title_Pathway_Grade_LastUpdatedOn,
            Value = PathwayGradeLastUpdatedOn.ToDobFormat(), // TODO
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