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

        [Required(ErrorMessageResourceType = typeof(AdminChangeMathsStatus), ErrorMessageResourceName = "Validation_Message")]
        public SubjectStatus? MathsStatusTo { get; set; }

        public bool IsLearnerRegisteredFourYearsAgo => DateTime.Now.Year - AcademicYear > 4;

        public SummaryItemModel SummaryLearner => new()
        {
            Id = "learner",
            Title = AdminChangeMathsStatus.Title_Learner_Text,
            Value = LearnerName
        };

        public SummaryItemModel SummaryULN => new()
        {
            Id = "uln",
            Title = AdminChangeMathsStatus.Title_ULN_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryProvider => new()
        {
            Id = "provider",
            Title = AdminChangeMathsStatus.Title_Provider_Text,
            Value = Provider
        };

        public SummaryItemModel SummaryTlevel => new()
        {
            Id = "tlevel",
            Title = AdminChangeMathsStatus.Title_TLevel_Text,
            Value = TlevelName
        };

        public SummaryItemModel SummaryAcademicYear => new()
        {
            Id = "academicyear",
            Title = AdminChangeMathsStatus.Title_StartYear_Text,
            Value = StartYear
        };

        public SummaryItemModel SummaryMathsStatus => new()
        {
            Id = "mathsstatus",
            Title = AdminChangeMathsStatus.Title_Maths_Status,
            Value = GetSubjectStatusDisplayText(MathsStatus)
        };

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminLearnerRecord,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
        };

        public string GetSubjectStatusDisplayText(SubjectStatus? status) => status switch
        {
            SubjectStatus.Achieved => AdminChangeMathsStatus.Status_Achieved_Text,
            SubjectStatus.NotAchieved => AdminChangeMathsStatus.Status_NotAchieved_Text,
            SubjectStatus.AchievedByLrs => AdminChangeMathsStatus.Status_AchievedByLrs_Text,
            SubjectStatus.NotAchievedByLrs => AdminChangeMathsStatus.Status_NotAchievedByLrs_Text,
            _ => AdminChangeMathsStatus.Status_Not_Yet_Received_Text
        };
    }
}
