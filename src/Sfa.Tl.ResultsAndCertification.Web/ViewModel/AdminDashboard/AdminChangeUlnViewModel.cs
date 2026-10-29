using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminChangeUlnViewModel
    {
        public int RegistrationPathwayId { get; set; }
        public long Uln { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProviderName { get; set; }
        public long ProviderUkprn { get; set; }
        public string TlevelName { get; set; }
        public int AcademicYear { get; set; }
        public string DisplayAcademicYear { get; set; }
        public string Learner => $"{FirstName} {LastName}";

        [Required(ErrorMessageResourceType = typeof(ChangeUln), ErrorMessageResourceName = "Validation_Uln_Must_Be_Digits")]
        [RegularExpression("^((?!(0))[0-9]{10})$", ErrorMessageResourceType = typeof(ChangeUln), ErrorMessageResourceName = "Validation_Uln_Must_Be_Digits")]
        public string UlnTo { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminLearnerRecord,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
        };

        public SummaryItemModel SummaryLearner => new()
        {
            Id = "learner",
            Title = ChangeUln.Title_Learner_Text,
            Value = Learner
        };

        public SummaryItemModel SummaryULN => new()
        {
            Id = "uln",
            Title = ChangeUln.Title_ULN_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryProvider => new()
        {
            Id = "provider",
            Title = ChangeUln.Title_Provider_Text,
            Value = $"{ProviderName} ({ProviderUkprn})"
        };

        public SummaryItemModel SummaryTlevel => new()
        {
            Id = "tlevel",
            Title = ChangeUln.Title_TLevel_Text,
            Value = TlevelName
        };

        public SummaryItemModel SummaryAcademicYear => new()
        {
            Id = "academicyear",
            Title = ChangeUln.Title_StartYear_Text,
            Value = DisplayAcademicYear
        };
    }
}
