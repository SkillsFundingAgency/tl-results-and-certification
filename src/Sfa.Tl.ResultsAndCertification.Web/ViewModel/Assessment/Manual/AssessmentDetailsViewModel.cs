using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.NotificationBanner;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.Linq;
using AssessmentDetailsContent = Sfa.Tl.ResultsAndCertification.Web.Content.Assessment.AssessmentDetails;
using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual
{
    public class AssessmentDetailsViewModel : AssessmentBaseViewModel
    {
        public AssessmentDetailsViewModel()
        {
            UlnLabel = AssessmentDetailsContent.Title_Uln_Text;
            LearnerNameLabel = AssessmentDetailsContent.Title_Name_Text;
            DateofBirthLabel = AssessmentDetailsContent.Title_DateofBirth_Text;
            ProviderNameLabel = AssessmentDetailsContent.Title_Provider_Text;
            TlevelTitleLabel = AssessmentDetailsContent.Title_TLevel_Text;
        }

        public NotificationBannerModel SuccessBanner { get; set; }

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

        public int? CurrentSpecialismAssessmentSeriesId { get; set; }
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
        public string CombinedSpecialismDisplayName => DisplayMultipleSpecialismsCombined ? string.Join(Constants.AndSeperator, SpecialismDetails.OrderBy(x => x.Name).Select(x => $"{x.Name} ({x.LarId})")) : null;
        public List<SpecialismViewModel> DisplaySpecialisms => DisplayMultipleSpecialismsCombined ? SpecialismDetails.Take(1).ToList() : SpecialismDetails;

        public SummaryItemModel SummaryExamPeriod
        {
            get
            {
                return HasResultForCurrentCoreAssessment ?
                    new SummaryItemModel
                    {
                        Id = "examperiod",
                        Title = AssessmentDetailsContent.Title_Exam_Period,
                        Value = PathwayAssessment?.SeriesName
                    }
                    :
                    new SummaryItemModel
                    {
                        Id = "examperiod",
                        Title = AssessmentDetailsContent.Title_Exam_Period,
                        Value = PathwayAssessment?.SeriesName,
                        ActionText = AssessmentDetailsContent.Remove_Action_Link_Text,
                        HiddenActionText = AssessmentDetailsContent.Remove_Action_Link_Hidden_Text,
                        RouteName = RouteConstants.RemoveCoreAssessmentEntry,
                        RouteAttributes = new Dictionary<string, string> { { Constants.AssessmentId, PathwayAssessment?.AssessmentId.ToString() } }
                    };
            }
        }

        public SummaryItemModel SummaryLastUpdatedOn => new SummaryItemModel
        {
            Id = "lastupdatedon",
            Title = AssessmentDetailsContent.Title_Last_Updated_On,
            Value = PathwayAssessment?.LastUpdatedOn.ToDobFormat()
        };

        public SummaryItemModel SummaryLastUpdatedBy => new SummaryItemModel
        {
            Id = "lastupdatedby",
            Title = AssessmentDetailsContent.Title_Last_Updated_By,
            Value = PathwayAssessment?.LastUpdatedBy
        };

        public SummaryItemModel GetSummaryExamPeriod(SpecialismViewModel specialismViewModel)
        {
            return new SummaryItemModel
            {
                Id = $"examperiod_{specialismViewModel.Id}",
                Title = AssessmentDetailsContent.Title_Exam_Period,
                Value = specialismViewModel.Assessments.FirstOrDefault(a => a.SeriesId == CurrentSpecialismAssessmentSeriesId).SeriesName,
                ActionText = AssessmentDetailsContent.Remove_Action_Link_Text,
                HiddenActionText = AssessmentDetailsContent.Remove_Action_Link_Hidden_Text
            };
        }

        public SummaryItemModel GetSummaryLastUpdatedOn(SpecialismViewModel specialismViewModel)
        {
            return new SummaryItemModel
            {
                Id = $"lastupdatedon_{specialismViewModel.Id}",
                Title = AssessmentDetailsContent.Title_Last_Updated_On,
                Value = specialismViewModel.Assessments.FirstOrDefault(a => a.SeriesId == CurrentSpecialismAssessmentSeriesId).LastUpdatedOn.ToDobFormat()
            };
        }

        public SummaryItemModel GetSummaryLastUpdatedBy(SpecialismViewModel specialismViewModel)
        {
            return new SummaryItemModel
            {
                Id = $"lastupdatedby_{specialismViewModel.Id}",
                Title = AssessmentDetailsContent.Title_Last_Updated_By,
                Value = specialismViewModel.Assessments.FirstOrDefault(a => a.SeriesId == CurrentSpecialismAssessmentSeriesId).LastUpdatedBy
            };
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
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Assessment_Dashboard, RouteName = RouteConstants.AssessmentDashboard },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Search_For_Assessments, RouteName = RouteConstants.SearchAssessments },
                        new BreadcrumbItem { DisplayName = BreadcrumbContent.Learners_Assessment_entries }
                    }
                };
            }
        }
    }
}
