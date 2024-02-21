using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.Assessment
{
    public class AdminAssessmentLearnerDetails
    {
        public int RegistrationPathwayId { get; set; }

        public string LearnerName { get; set; }

        public long Uln { get; set; }

        public string Provider { get; set; }

        public string TlevelName { get; set; }

        public int StartYear { get; set; }

        public string DisplayStartYear { get; set; }

        public string PathwayDisplayName { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminLearnerAssessmentEntry), ErrorMessageResourceName = "Validation_Message")]
        public string AssessmentYearTo { get; set; }


        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminLearnerRecord,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
        };

        public SummaryItemModel SummaryLearner => new()
        {
            Id = "learner",
            Title = AdminLearnerAssessmentEntry.Title_Learner_Text,
            Value = LearnerName
        };

        public SummaryItemModel SummaryULN => new()
        {
            Id = "uln",
            Title = AdminLearnerAssessmentEntry.Title_ULN_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryProvider => new()
        {
            Id = "provider",
            Title = AdminLearnerAssessmentEntry.Title_Provider_Text,
            Value = Provider
        };

        public SummaryItemModel SummaryTlevel => new()
        {
            Id = "tlevel",
            Title = AdminLearnerAssessmentEntry.Title_TLevel_Text,
            Value = TlevelName
        };
        public SummaryItemModel SummaryAcademicYear => new()
        {
            Id = "startyear",
            Title = AdminLearnerAssessmentEntry.Title_StartYear_Text,
            Value = DisplayStartYear
        };
    }
}
