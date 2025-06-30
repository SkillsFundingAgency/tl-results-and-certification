using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LevelTwoResults
{
    public class AdminChangeMathsResultsViewModel
    {
        public int RegistrationPathwayId { get; set; }
        public string LearnerName { get; set; }
        public long Uln { get; set; }
        public string Provider { get; set; }
        public string TlevelName { get; set; }
        public int AcademicYear { get; set; }
        public string StartYear { get; set; }
        public SubjectStatus? MathsStatus { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminChangeLevelTwoMaths), ErrorMessageResourceName = "Validation_Message")]
        public SubjectStatus? MathsStatusTo { get; set; }

        public bool IsLearnerRegisteredFourYearsAgo => DateTime.Now.Year - AcademicYear > 4;

        public SummaryItemModel SummaryLearner => new()
        {
            Id = "learner",
            Title = AdminChangeLevelTwoMaths.Title_Learner_Text,
            Value = LearnerName
        };

        public SummaryItemModel SummaryULN => new()
        {
            Id = "uln",
            Title = AdminChangeLevelTwoMaths.Title_ULN_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryProvider => new()
        {
            Id = "provider",
            Title = AdminChangeLevelTwoMaths.Title_Provider_Text,
            Value = Provider
        };

        public SummaryItemModel SummaryTlevel => new()
        {
            Id = "tlevel",
            Title = AdminChangeLevelTwoMaths.Title_TLevel_Text,
            Value = TlevelName
        };

        public SummaryItemModel SummaryAcademicYear => new()
        {
            Id = "academicyear",
            Title = AdminChangeLevelTwoMaths.Title_StartYear_Text,
            Value = StartYear
        };

        public SummaryItemModel SummaryMathsStatus => new()
        {
            Id = "mathsstatus",
            Title = AdminChangeLevelTwoMaths.Title_Maths_Status,
            Value = GetSubjectStatusDisplayText(MathsStatus)
        };

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminLearnerRecord,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
        };

        public string GetSubjectStatusDisplayText(SubjectStatus? status) => status switch
        {
            SubjectStatus.Achieved => AdminChangeLevelTwoMaths.Status_Achieved_Text,
            SubjectStatus.NotAchieved => AdminChangeLevelTwoMaths.Status_NotAchieved_Text,
            SubjectStatus.AchievedByLrs => AdminChangeLevelTwoMaths.Status_AchievedByLrs_Text,
            SubjectStatus.NotAchievedByLrs => AdminChangeLevelTwoMaths.Status_NotAchievedByLrs_Text,
            _ => AdminChangeLevelTwoMaths.Status_Not_Yet_Received_Text
        };
    }
}
