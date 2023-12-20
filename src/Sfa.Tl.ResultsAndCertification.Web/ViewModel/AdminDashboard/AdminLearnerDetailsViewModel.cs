using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System.Collections.Generic;
using ChangeStarYear = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.ChangeStartYear;

using BreadcrumbContent = Sfa.Tl.ResultsAndCertification.Web.Content.ViewComponents.Breadcrumb;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard
{
    public class AdminLearnerDetailsViewModel
    {
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Learner => $"{FirstName} {LastName}";
        public long Uln { get; set; }
        public string Provider { get; set; }
        public string StartYear { get; set; }

        public BreadcrumbModel Breadcrumb => new()
        {
            BreadcrumbItems = new List<BreadcrumbItem>
            {
                new BreadcrumbItem { DisplayName = BreadcrumbContent.Home, RouteName = RouteConstants.Home }
            }
        };

        public SummaryItemModel SummaryLearner => new()
        {
            Id = "learner",
            Title = ChangeStarYear.Title_ULN_Text,
            Value = Learner
        };

        public SummaryItemModel SummaryULN => new()
        {
            Id = "uln",
            Title = ChangeStarYear.Title_ULN_Text,
            Value = Uln.ToString()
        };

        public SummaryItemModel SummaryProvider => new()
        {
            Id = "provider",
            Title = ChangeStarYear.Title_Provider_Text,
            Value = Provider
        };

        public SummaryItemModel SummaryStartYear => new()
        {
            Id = "startyear",
            Title = ChangeStarYear.Title_StartYear_Text,
            Value = StartYear
        };
    }
}
