using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppealGradeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAppealGradeAfterDeadlineConfirm;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    // TODO: Ravi - To be deleted?
    public class AppealGradeAfterDeadlineConfirmViewModel : PrsBaseViewModel
    {
        public AppealGradeAfterDeadlineConfirmViewModel()
        {
            UlnLabel = AppealGradeContent.Title_Uln_Text;
            LearnerNameLabel = AppealGradeContent.Title_Name_Text;
            DateofBirthLabel = AppealGradeContent.Title_DateofBirth_Text;
        }

        public int ProfileId { get; set; }
        public int PathwayAssessmentId { get; set; }
        public int PathwayResultId { get; set; }
        public string PathwayDisplayName { get; set; }
        public string PathwayAssessmentSeries { get; set; }
        public string PathwayGrade { get; set; }
        public PrsStatus? PathwayPrsStatus { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
        public DateTime AppealEndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(AppealGradeContent), ErrorMessageResourceName = "Validation_Message")]
        public bool? IsThisGradeBeingAppealed { get; set; }

        public bool IsValid { get { return Status == RegistrationPathwayStatus.Active && IsAppealAllowedAfterDeadline; } }

        public SummaryItemModel SummaryCore => new SummaryItemModel
        {
            Id = "core",
            Title = AppealGradeContent.Title_Core_Text,
            Value = PathwayDisplayName,
            IsRawHtml = true
        };

        public SummaryItemModel SummaryCoreExamPeriod => new SummaryItemModel
        {
            Id = "coreexamperiod",
            Title = AppealGradeContent.Title_ExamPeriod_Text,
            Value = PathwayAssessmentSeries
        };

        public SummaryItemModel SummaryCoreGrade => new SummaryItemModel
        {
            Id = "coregrade",
            Title = AppealGradeContent.Title_Grade_Text,
            Value = PathwayGrade
        };

        public override BackLinkModel BackLink => new BackLinkModel
        {
            //RouteName = RouteConstants.PrsAppealGradeAfterDeadline,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.AssessmentId, PathwayAssessmentId.ToString() } }
        };

        private bool IsAppealAllowedAfterDeadline { get { return !PathwayPrsStatus.HasValue && !CommonHelper.IsAppealsAllowed(AppealEndDate); } }
    }
}