using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GradeChangeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsGradeChangeRequest;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsGradeChangeRequestViewModel : PrsBaseViewModel
    {
        public PrsGradeChangeRequestViewModel()
        {
            UlnLabel = GradeChangeContent.Title_Uln_Text;
            LearnerNameLabel = GradeChangeContent.Title_Name_Text;
            DateofBirthLabel = GradeChangeContent.Title_DateofBirth_Text;
        }

        public bool CanRequestFinalGradeChange { get { return Status == RegistrationPathwayStatus.Active && PathwayPrsStatus == PrsStatus.Final; } }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ResultId { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
        public PrsStatus PathwayPrsStatus { get; set; }

        public string PathwayDisplayName { get; set; }
        public string PathwayAssessmentSeries { get; set; }
        public string PathwayGrade { get; set; }

        [Required(ErrorMessageResourceType = typeof(GradeChangeContent), ErrorMessageResourceName = "Validation_Message")]
        public string ChangeRequestData { get; set; }

        public bool? IsResultJourney { get; set; }

        public SummaryItemModel SummaryCore => new SummaryItemModel
        {
            Id = "core",
            Title = GradeChangeContent.Title_Core_Text,
            Value = PathwayDisplayName,
            IsRawHtml = true
        };

        public SummaryItemModel SummaryCoreExamPeriod => new SummaryItemModel
        {
            Id = "coreexamperiod",
            Title = GradeChangeContent.Title_ExamPeriod_Text,
            Value = PathwayAssessmentSeries
        };

        public SummaryItemModel SummaryCoreGrade => new SummaryItemModel
        {
            Id = "coregrade",
            Title = GradeChangeContent.Title_Grade_Text,
            Value = PathwayGrade
        };

        public override BackLinkModel BackLink =>
            IsResultJourney.HasValue && IsResultJourney == true ? new BackLinkModel
            {
                RouteName = RouteConstants.ResultDetails,
                RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
            }
            : new BackLinkModel
            {
                RouteName = RouteConstants.PrsLearnerDetails,
                RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.AssessmentId, AssessmentId.ToString() } }
            };
    }
}
