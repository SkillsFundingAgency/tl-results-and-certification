using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.Utilities.CustomValidations;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ErrorResource = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class ReviewChangeUlnViewModel
    {
        public int RegistrationPathwayId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Uln { get; set; }
        public string ProviderName { get; set; }
        public int ProviderUkprn { get; set; }
        public string TlevelName { get; set; }
        public string Learner => $"{FirstName} {LastName}";

        [Required(ErrorMessageResourceType = typeof(ErrorResource.ReviewChangeUln), ErrorMessageResourceName = "Validation_Contact_Name_Blank_Text")]
        [StringLength(100)]
        public string ContactName { get; set; }

        [DateValidator(Property = nameof(RequestDate), ErrorResourceType = typeof(ReviewChangeUln))]
        public string RequestDate => $"{Year}/{Month}/{Day}";
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorResource.ReviewChangeUln), ErrorMessageResourceName = "Validation_Reason_For_Change_Blank_Text")]
        [StringLength(2000)]
        public string ChangeReason { get; set; }

        [StringLength(10)]
        public string ZendeskId { get; set; }

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminChangeUln,
            RouteAttributes = new Dictionary<string, string>() { { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() } }
        };

        public SummaryItemModel SummaryLearner => new()
        {
            Id = "learner",
            Title = ReviewChangeUln.Title_Learner_Text,
            Value = Learner
        };

        public SummaryItemModel SummaryULN => new()
        {
            Id = "uln",
            Title = ReviewChangeUln.Title_ULN_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryProvider => new()
        {
            Id = "provider",
            Title = ReviewChangeUln.Title_Provider_Text,
            Value = $"{ProviderName} ({ProviderUkprn})"
        };

        public SummaryItemModel SummaryTlevel => new()
        {
            Id = "tlevel",
            Title = ReviewChangeUln.Title_TLevel_Text,
            Value = TlevelName
        };
        public string UlnTo { get; set; }
        public SummaryItemModel SummaryUlnChange => new()
        {
            Id = "ulnchange", Title = ReviewChangeUln.Title_ULN_Text, Value = Uln.ToString(), Value2 = UlnTo,
            ActionText = ReviewChangeUln.Link_Change_Text, RouteName = RouteConstants.AdminChangeUln,
            RouteAttributes = new Dictionary<string, string>() { { Constants.RegistrationPathwayId, RegistrationPathwayId.ToString() } }
        };

    }
}
