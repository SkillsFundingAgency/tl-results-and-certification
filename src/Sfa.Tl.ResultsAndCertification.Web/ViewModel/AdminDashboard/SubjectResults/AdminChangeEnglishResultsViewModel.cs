using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.SubjectResults
{
    public class AdminChangeEnglishResultsViewModel
    {
        public int RegistrationPathwayId { get; set; }
        public string LearnerName { get; set; }
        public long Uln { get; set; }
        public string Provider { get; set; }
        public string TlevelName { get; set; }
        public int AcademicYear { get; set; }
        public string StartYear { get; set; }
        public SubjectStatus? EnglishStatus { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminChangeLevelTwoEnglish), ErrorMessageResourceName = "Validation_Message")]
        public SubjectStatus? EnglishStatusTo { get; set; }

        public bool IsLearnerRegisteredFourYearsAgo => DateTime.Now.Year - AcademicYear > 4;

        public SummaryItemModel SummaryLearner => new()
        {
            Id = "learner",
            Title = AdminChangeLevelTwoEnglish.Title_Learner_Text,
            Value = LearnerName
        };

        public SummaryItemModel SummaryULN => new()
        {
            Id = "uln",
            Title = AdminChangeLevelTwoEnglish.Title_ULN_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryProvider => new()
        {
            Id = "provider",
            Title = AdminChangeLevelTwoEnglish.Title_Provider_Text,
            Value = Provider
        };

        public SummaryItemModel SummaryTlevel => new()
        {
            Id = "tlevel",
            Title = AdminChangeLevelTwoEnglish.Title_TLevel_Text,
            Value = TlevelName
        };

        public SummaryItemModel SummaryAcademicYear => new()
        {
            Id = "academicyear",
            Title = AdminChangeLevelTwoEnglish.Title_StartYear_Text,
            Value = StartYear
        };

        public SummaryItemModel SummaryEnglishStatus => new()
        {
            Id = "englishstatus",
            Title = AdminChangeLevelTwoEnglish.Title_English_Status,
            Value = GetSubjectStatusDisplayText(EnglishStatus)
        };

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminLearnerRecord,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
        };

        public string GetSubjectStatusDisplayText(SubjectStatus? status) => status switch
        {
            SubjectStatus.Achieved => AdminChangeLevelTwoEnglish.Status_Achieved_Text,
            SubjectStatus.NotAchieved => AdminChangeLevelTwoEnglish.Status_NotAchieved_Text,
            SubjectStatus.AchievedByLrs => AdminChangeLevelTwoEnglish.Status_AchievedByLrs_Text,
            SubjectStatus.NotAchievedByLrs => AdminChangeLevelTwoEnglish.Status_NotAchievedByLrs_Text,
            _ => AdminChangeLevelTwoEnglish.Status_Not_Yet_Received_Text
        };
    }
}
