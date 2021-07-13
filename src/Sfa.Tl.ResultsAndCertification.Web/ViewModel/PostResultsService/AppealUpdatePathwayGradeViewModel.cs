using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppealUpdatePathwayGradeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.AppealUpdatePathwayGrade;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class AppealUpdatePathwayGradeViewModel
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

        [Required(ErrorMessageResourceType = typeof(AppealUpdatePathwayGradeContent), ErrorMessageResourceName = "Validation_Message")]
        public string SelectedGradeCode { get; set; }

        public List<LookupViewModel> Grades { get; set; }
        public bool IsChangeMode { get; set; }

        public bool IsValid => PathwayPrsStatus == PrsStatus.BeingAppealed;

        public SummaryItemModel SummaryUln => new SummaryItemModel
        {
            Id = "uln",
            Title = AppealUpdatePathwayGradeContent.Title_Uln_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryLearnerName => new SummaryItemModel
        {
            Id = "learnername",
            Title = AppealUpdatePathwayGradeContent.Title_Name_Text,
            Value = LearnerName
        };

        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel
        {
            Id = "dateofbirth",
            Title = AppealUpdatePathwayGradeContent.Title_DateofBirth_Text,
            Value = DateofBirth.ToDobFormat()
        };

        public SummaryItemModel SummaryCore => new SummaryItemModel
        {
            Id = "core",
            Title = AppealUpdatePathwayGradeContent.Title_Core_Text,
            Value = PathwayDisplayName,
            IsRawHtml = true
        };

        public SummaryItemModel SummaryCoreExamPeriod => new SummaryItemModel
        {
            Id = "coreexamperiod",
            Title = AppealUpdatePathwayGradeContent.Title_ExamPeriod_Text,
            Value = PathwayAssessmentSeries
        };

        public SummaryItemModel SummaryCoreGrade => new SummaryItemModel
        {
            Id = "coregrade",
            Title = AppealUpdatePathwayGradeContent.Title_Grade_Text,
            Value = PathwayGrade
        };

        public virtual BackLinkModel BackLink => new BackLinkModel
        {
            RouteName = IsChangeMode ? RouteConstants.PrsPathwayGradeCheckAndSubmit : RouteConstants.PrsAppealOutcomePathwayGrade,
            RouteAttributes = IsChangeMode ? null 
            : new Dictionary<string, string>
            {
                { Constants.ProfileId, ProfileId.ToString() },
                { Constants.AssessmentId, PathwayAssessmentId.ToString() },
                { Constants.ResultId, PathwayResultId.ToString() },
                { Constants.AppealOutcomeTypeId, ((int)AppealOutcomeType.UpdateGrade).ToString() }
            }
        };
    }
}
