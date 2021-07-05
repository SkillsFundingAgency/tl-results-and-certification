using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppealCoreGradeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.AppealCoreGrade;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class AppealOutcomePathwayGradeViewModel
    {
        public int ProfileId { get; set; }
        public int PathwayAssessmentId { get; set; }
        public int PathwayResultId { get; set; }        
        public long Uln { get; set; }
        public string LearnerName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string PathwayName { get; set; }
        public string PathwayCode { get; set; }
        public string PathwayDisplayName { get; set; }
        public string PathwayAssessmentSeries { get; set; }
        public string PathwayGrade { get; set; }
        public PrsStatus? PathwayPrsStatus { get; set; }

        [Required(ErrorMessageResourceType = typeof(AppealCoreGradeContent), ErrorMessageResourceName = "Validation_Message")]
        public AppealOutcomeType? AppealOutcome { get; set; }

        public bool IsValid => PathwayPrsStatus == PrsStatus.BeingAppealed;

        public SummaryItemModel SummaryUln => new SummaryItemModel
        {
            Id = "uln",
            Title = AppealCoreGradeContent.Title_Uln_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryLearnerName => new SummaryItemModel
        {
            Id = "learnername",
            Title = AppealCoreGradeContent.Title_Name_Text,
            Value = LearnerName
        };

        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel
        {
            Id = "dateofbirth",
            Title = AppealCoreGradeContent.Title_DateofBirth_Text,
            Value = DateofBirth.ToDobFormat()
        };

        public SummaryItemModel SummaryCore => new SummaryItemModel
        {
            Id = "core",
            Title = AppealCoreGradeContent.Title_Core_Text,
            Value = PathwayDisplayName,
            IsRawHtml = true
        };

        public SummaryItemModel SummaryCoreExamPeriod => new SummaryItemModel
        {
            Id = "coreexamperiod",
            Title = AppealCoreGradeContent.Title_ExamPeriod_Text,
            Value = PathwayAssessmentSeries
        };

        public SummaryItemModel SummaryCoreGrade => new SummaryItemModel
        {
            Id = "coregrade",
            Title = AppealCoreGradeContent.Title_Grade_Text,
            Value = PathwayGrade
        };

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.PrsLearnerDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() }, { Constants.AssessmentId, PathwayAssessmentId.ToString() } }
        };
    }
}
