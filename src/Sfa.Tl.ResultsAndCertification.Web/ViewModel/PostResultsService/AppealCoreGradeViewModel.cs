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
    public class AppealCoreGradeViewModel
    {
        public int ProfileId { get; set; }
        public int CoreResultId { get; set; }
        public long Uln { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateofBirth { get; set; }
        public string CoreName { get; set; }
        public long CoreCode { get; set; }
        public string CoreAssessmentSeries { get; set; }
        public string CoreGrade { get; set; }

        [Required(ErrorMessageResourceType = typeof(AppealCoreGradeContent), ErrorMessageResourceName = "Validation_Message")]
        public bool? AppealGrade { get; set; }

        private string LearnerName => $"{Firstname} {Lastname}";
        private string CoreDisplayName => $"{CoreName}<br/>({CoreCode})";

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
            Value = CoreDisplayName,
            IsRawHtml = true
        };

        public SummaryItemModel SummaryCoreExamPeriod => new SummaryItemModel
        {
            Id = "coreexamperiod",
            Title = AppealCoreGradeContent.Title_ExamPeriod_Text,
            Value = CoreAssessmentSeries
        };

        public SummaryItemModel SummaryCoreGrade => new SummaryItemModel
        {
            Id = "coregrade",
            Title = AppealCoreGradeContent.Title_Grade_Text,
            Value = CoreGrade
        };

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = RouteConstants.PrsReviewsAndAppealsStatus,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }
}