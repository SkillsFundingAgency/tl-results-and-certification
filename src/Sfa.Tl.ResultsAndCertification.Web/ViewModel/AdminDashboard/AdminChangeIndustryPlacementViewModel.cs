using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using IpStatus = Sfa.Tl.ResultsAndCertification.Common.Enum.IndustryPlacementStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminChangeIndustryPlacementViewModel
    {
        public int ProfileId { get; set; }
        public int RegistrationPathwayId { get; set; }
        public int PathwayId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long Uln { get; set; }
        public string ProviderName { get; set; }
        public int ProviderUkprn { get; set; }
        public string TlevelName { get; set; }
        public int AcademicYear { get; set; }
        public string DisplayAcademicYear { get; set; } 
        public string Learner => $"{FirstName} {LastName}";
        public string LearnerRegistrationPathwayStatus { get; set; }

        [Required(ErrorMessageResourceType = typeof(AdminChangeIndustryPlacement), ErrorMessageResourceName = "Validation_Message")]
        public IndustryPlacementStatus? IndustryPlacementStatus { get; set; }

        public bool IsLearnerRegisteredFourYearsAgo => (DateTime.Now.Year - AcademicYear) > 4;

        public BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.AdminLearnerRecord,
            RouteAttributes = new Dictionary<string, string> { { Constants.PathwayId, RegistrationPathwayId.ToString() } }
        };

        public SummaryItemModel SummaryLearner => new()
        {
            Id = "learner",
            Title = AdminChangeIndustryPlacement.Title_Learner_Text,
            Value = Learner
        };

        public SummaryItemModel SummaryULN => new()
        {
            Id = "uln",
            Title = AdminChangeIndustryPlacement.Title_ULN_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryProvider => new()
        {
            Id = "provider",
            Title = AdminChangeIndustryPlacement.Title_Provider_Text,
            Value = $"{ProviderName} ({ProviderUkprn})"
        };

        public SummaryItemModel SummaryTlevel => new()
        {
            Id = "tlevel",
            Title = AdminChangeIndustryPlacement.Title_TLevel_Text,
            Value = TlevelName
        };
        public SummaryItemModel SummaryAcademicYear => new()
        {
            Id = "academicyear",
            Title = AdminChangeIndustryPlacement.Title_StartYear_Text,
            Value = DisplayAcademicYear
        };

        public SummaryItemModel SummaryIndustryPlacementStatus => new()
        {
            Id = "industryplacement",
            Title = AdminChangeIndustryPlacement.Title_Industry_Placement_Status,
            Value = GetIndustryPlacementDisplayText
        };

        public string GetIndustryPlacementDisplayText => IndustryPlacementStatus switch
        {
            IpStatus.Completed => AdminChangeIndustryPlacement.Status_Yes_Completed,
            IpStatus.CompletedWithSpecialConsideration => AdminChangeIndustryPlacement.Status_Yes_Completed_with_Special_Consideration,
            IpStatus.NotCompleted => AdminChangeIndustryPlacement.Status_No_They_Still_Need_To_Complete_It,
            IpStatus.WillNotComplete => AdminChangeIndustryPlacement.Status_No_They_Will_Not_Complete_It,
            _ => AdminChangeIndustryPlacement.Staut_Not_Yet_Recieved,
        };

    }
}