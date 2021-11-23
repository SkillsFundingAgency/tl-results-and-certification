using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using System.Collections.Generic;
using System.Linq;
using AssessmentDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AssessmentDetails;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class LearnerAssessmentDetailsViewModel : AssessmentBaseViewModel
    {
        public LearnerAssessmentDetailsViewModel()
        {
            UlnLabel = AssessmentDetailsContent.Title_Uln_Text;
            LearnerNameLabel = AssessmentDetailsContent.Title_Name_Text;
            DateofBirthLabel = AssessmentDetailsContent.Title_DateofBirth_Text;
            ProviderNameLabel = AssessmentDetailsContent.Title_Provider_Text;
            TlevelTitleLabel = AssessmentDetailsContent.Title_TLevel_Text;
        }

        public RegistrationPathwayStatus PathwayStatus { get; set; }

        public int PathwayId { get; set; }
        public string PathwayDisplayName { get; set; }
        public PathwayAssessmentViewModel PathwayAssessment { get; set; }
        public PathwayAssessmentViewModel PreviousPathwayAssessment { get; set; }
        public bool IsCoreEntryEligible { get; set; }
        public string NextAvailableCoreSeries { get; set; }
        public bool IsCoreResultExist { get; set; }
        public bool HasAnyOutstandingPathwayPrsActivities { get; set; }
        public bool IsIndustryPlacementExist { get; set; }
        
        public List<SpecialismViewModel> SpecialismDetails { get; set; }
        public bool IsSpecialismEntryEligible { get; set; }
        public string NextAvailableSpecialismSeries { get; set; }
        public bool HasCurrentSpecialismAssessmentEntry { get; set; }
        public bool IsResitForSpecialism { get; set; }

        public bool HasCurrentCoreAssessmentEntry => PathwayAssessment != null;
        public bool HasResultForCurrentCoreAssessment => HasCurrentCoreAssessmentEntry && PathwayAssessment.Results.Any();
        public bool HasPreviousCoreAssessment => PreviousPathwayAssessment != null;
        public bool HasResultForPreviousCoreAssessment => HasPreviousCoreAssessment && PreviousPathwayAssessment.Results.Any();
        public bool NeedCoreResultForPreviousAssessmentEntry => !HasCurrentCoreAssessmentEntry && HasPreviousCoreAssessment && !HasResultForPreviousCoreAssessment;                
        public bool DisplayMultipleSpecialismsCombined => SpecialismDetails.Count > 1 && !IsResitForSpecialism;

        public bool IsSpecialismRegistered => SpecialismDetails.Any();
        public string SpecialismDisplayName => DisplayMultipleSpecialismsCombined ? string.Join(Constants.AndSeperator, SpecialismDetails.OrderBy(x => x.Name).Select(x => $"{x.Name} ({x.LarId})")) : null;
        public List<SpecialismViewModel> DisplaySpecialisms => DisplayMultipleSpecialismsCombined ? SpecialismDetails.Take(1).ToList() : SpecialismDetails;

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
